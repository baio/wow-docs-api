namespace FSharp.Dapr

open Microsoft.AspNetCore.Hosting

[<AutoOpen>]
module App =
    open Microsoft.AspNetCore.Http
    open Microsoft.Extensions.Logging
    open Dapr.Client
    open Microsoft.Extensions.Configuration
    open Saturn
    open System.Text.Json.Serialization
    open Giraffe
    open System.Text.Json
    open FSharp.Control.Tasks

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

    let daprApp'
        (webhostConfig: IConfiguration -> IWebHostBuilder -> IWebHostBuilder)
        (envFactory: DaprAppEnv * HttpContext -> 'x)
        (defaultAppPort)
        (app: DaprApp<'x>)
        =

        let dapr = DaprClientBuilder().Build()

        let envFactory' (httpContext: HttpContext) =
            envFactory (
                { Logger = httpContext.GetLogger()
                  Dapr = dapr },
                httpContext
            )

        let routes =
            match app with
            | DaprSubs subs -> subsToHandler envFactory' subs
            | DaprRouter router -> fun next ctx -> router (envFactory' ctx) next ctx

        // Make all parts to talk same json

        // https://giraffe.wiki/docs#json
        // https://github.com/Tarmil/FSharp.SystemTextJson
        // set json converter for dapr https://github.com/dapr/dotnet-sdk/issues/362

        //
        let converter =
            JsonFSharpConverter(JsonUnionEncoding.FSharpLuLike)

        dapr.JsonSerializerOptions.Converters.Add(converter)
        //
        let serializationOptions =
            JsonSerializerOptions(PropertyNameCaseInsensitive = true)

        serializationOptions.Converters.Add(converter)

        let serializer =
            SystemTextJson.Serializer(serializationOptions)
        //
        let config =
            ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build()

        let appBuilder =
            application {
                use_config (fun _ -> config)
                use_router routes
                url (getAppUrl defaultAppPort)
                use_gzip
                use_json_serializer (serializer)
                webhost_config (webhostConfig config)
            }

        appBuilder

    let daprApp webhostConfig = daprApp' webhostConfig (fst)
