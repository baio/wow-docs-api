namespace FSharp.Dapr

[<AutoOpen>]
module PubSub =
    open Giraffe
    open Dapr.Client
    open FSharp.Control.Tasks
    open System.Threading.Tasks

    type SubscribeHttpHandler<'x> = 'x -> HttpHandler

    let publishEventAsync<'a> pubSubName topicName { Dapr = dapr; Logger = logger } (event: 'a) =
        logTrace3 logger "Publishing {pubSubName} {topicName} {event}" pubSubName topicName event
        dapr.PublishEventAsync(pubSubName, topicName, event)

    let subscribeDapr'<'x, 'e> (pubSubName: string) (topicName: string) (handler: 'e -> SubscribeHttpHandler<'x>) =
        pubSubName,
        topicName,
        fun env next ctx ->
            task {
                let! event = bindCloudEventDataAsync<'e> ctx
                return! handler event env next ctx
            }

    let subscribeDapr<'x, 'e, 'r> pubSubName topicName (handler: 'e -> 'x -> Task<'r>) =
        subscribeDapr'<'x, 'e>
            pubSubName
            topicName
            (fun event dapr next ctx ->
                task {
                    let! resultEvent = handler event dapr
                    return! json resultEvent next ctx
                })
