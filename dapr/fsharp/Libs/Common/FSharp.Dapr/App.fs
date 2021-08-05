namespace FSharp.Dapr

[<AutoOpen>]
module App =
    open Microsoft.AspNetCore.Http
    open Microsoft.Extensions.Logging
    open Dapr.Client
    open Microsoft.Extensions.Configuration
    open Microsoft.AspNetCore.Hosting
    open Microsoft.Extensions.Hosting
    open System.Text.Json.Serialization
    open Giraffe
    open System.Text.Json
    open FSharp.Control.Tasks
    open Microsoft.AspNetCore.Builder
    open Microsoft.Extensions.DependencyInjection

    type PubSubName = string

    type TopicName = string

    type TopicHandler<'x> = 'x -> HttpHandler

    type DaprSubs<'x> = (PubSubName * TopicName * TopicHandler<'x>) List

    type DaprApp<'x> =
        | DaprSubs of DaprSubs<'x>
        | DaprRouter of TopicHandler<'x>

    let private subToHandler envFactory (pubSubName, topicName, topicHandler: TopicHandler<'x>) =
        POST
        >=> route $"/${pubSubName}/{topicName}"
        >=> (fun next ctx ->
            task {
                try
                    return! topicHandler (envFactory ctx) next ctx
                with
                | ex ->
                    ctx
                        .GetLogger()
                        .LogError("Error while handling, return ok to not retry. Error : {error}", ex)

                    return! Successful.ok (json {| ok = "OK but not OK, to not retry" |}) next ctx
            })

    let private getDaprSubscribeHandler (subs: DaprSubs<'x>) =
        subs
        |> List.map
            (fun (pubSub, topic, _) ->
                {| pubsubname = pubSub
                   topic = topic
                   route = $"/${pubSub}/{topic}" |})
        |> json

    let private getDaprSubscribeRouter (subs: DaprSubs<'x>) =
        GET
        >=> route "/dapr/subscribe"
        >=> getDaprSubscribeHandler subs

    let private subsToHandler dapr (subs: DaprSubs<'x>) =
        let subscribeRouter = getDaprSubscribeRouter subs
        let subToHandler = subToHandler dapr
        let routers = subs |> List.map (subToHandler)
        subscribeRouter :: routers |> choose

    let private configureApp webApp (app: IApplicationBuilder) =
        // Add Giraffe to the ASP.NET Core pipeline
        app.UseResponseCompression().UseGiraffe webApp

    let getJsonConverter () =
        JsonFSharpConverter(JsonUnionEncoding.FSharpLuLike, allowNullFields = true)

    let private configureServices (services: IServiceCollection) =
        // https://giraffe.wiki/docs#json
        // https://github.com/Tarmil/FSharp.SystemTextJson
        // set json converter for dapr https://github.com/dapr/dotnet-sdk/issues/362

        // Add Giraffe dependencies

        let serializationOptions =
            JsonSerializerOptions(PropertyNameCaseInsensitive = true)

        let converter = getJsonConverter ()
        serializationOptions.Converters.Add(converter)            

        let serializer = 
            SystemTextJson.Serializer(serializationOptions)

        services
            .AddGiraffe()
            .AddResponseCompression()
            .AddSingleton<Json.ISerializer>(serializer)
        |> ignore


    let configureWebHostBuilder webApp (webHostBuilder: IWebHostBuilder) =
        webHostBuilder
            .Configure(configureApp webApp)
            .ConfigureServices(configureServices)

    let dapr2webApp envFactory =
        function
        | DaprSubs subs -> subsToHandler envFactory subs
        | DaprRouter router -> fun next ctx -> router (envFactory ctx) next ctx

    let createDaprClient () =
        let daprClient = DaprClientBuilder().Build()
        let jsonConverter = getJsonConverter ()
        daprClient.JsonSerializerOptions.Converters.Add(jsonConverter)
        daprClient


    let runDaprApp'
        (webhostConfig: IConfiguration -> IWebHostBuilder -> IWebHostBuilder)
        (envFactory: DaprAppEnv * HttpContext -> 'x)
        (defaultAppPort)
        (app: DaprApp<'x>)
        =

        let dapr = createDaprClient()

        let envFactory' (httpContext: HttpContext) =
            envFactory (
                { Logger = httpContext.GetLogger()
                  Dapr = dapr },
                httpContext
            )

        let routes = dapr2webApp envFactory' app

        let config =
            ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional = true)
                .AddEnvironmentVariables()
                .Build()

        let url = getAppUrl defaultAppPort

        Host
            .CreateDefaultBuilder()
            .ConfigureWebHostDefaults(fun builder ->
                (configureWebHostBuilder routes builder)
                    .UseUrls([| url |])
                |> webhostConfig config
                |> ignore)
            .Build()
            .Run()

        0

    let runDaprApp webhostConfig = runDaprApp' webhostConfig (fst)
