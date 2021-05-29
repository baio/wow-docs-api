namespace Shared

[<AutoOpen>]
module DaprApp =

    open Dapr.Client
    open Microsoft.Extensions.Configuration
    open Saturn
    open Shared
    open System.Text.Json.Serialization
    open Giraffe
    open System.Text.Json

    type PubSubName = string

    type TopicName = string

    type TopicHandler = DaprClient -> HttpHandler

    type DaprSubs = (TopicName * TopicHandler) List

    type DaprApp =
    | DaprSubs of PubSubName * DaprSubs
    | DaprRouter of TopicHandler

    let private subToHandler dapr (topicName, topicHandler: TopicHandler) =
        POST >=> route $"/{topicName}" >=> topicHandler dapr

    let private getDaprSubscribeHandler pubSubName (subs: DaprSubs) = 
        subs
        |> List.map fst
        |> List.map (fun topic -> 
            {| 
                pubsubname = pubSubName
                topic = topic
                route = topic |}        
        ) |> json

    let private getDaprSubscribeRouter pubSubName (subs: DaprSubs) = 
        GET >=> route "/dapr/subscribe" >=> getDaprSubscribeHandler pubSubName subs

    let private subsToHandler dapr pubSubName (subs: DaprSubs) =
        let subscribeRouter = getDaprSubscribeRouter pubSubName subs
        let subToHandler = subToHandler dapr 
        let routers = subs |> List.map(subToHandler)         
        subscribeRouter::routers |> choose

    let daprApp (defaultAppPort) (app: DaprApp) =

        let dapr = DaprClientBuilder().Build()
        let routes =
            match app with
            | DaprSubs (pubSubName, subs) -> subsToHandler dapr pubSubName subs
            | DaprRouter router -> router dapr
                
        // Make all parts to talk same json 

        // https://giraffe.wiki/docs#json
        // https://github.com/Tarmil/FSharp.SystemTextJson
        // set json converter for dapr https://github.com/dapr/dotnet-sdk/issues/362

        //
        let converter = JsonFSharpConverter(JsonUnionEncoding.FSharpLuLike)
        dapr.JsonSerializerOptions.Converters.Add(converter)
        //
        let serializationOptions = JsonSerializerOptions(PropertyNameCaseInsensitive = true)
        serializationOptions.Converters.Add(converter)
        let serializer = SystemTextJson.Serializer(serializationOptions)
        //
        let config =
            ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build()

        application {
            use_config (fun _ -> config)
            use_router routes
            url (getAppUrl defaultAppPort)
            use_gzip
            use_json_serializer(serializer)
            webhost_config (createSerilogLogger config)   
        }
