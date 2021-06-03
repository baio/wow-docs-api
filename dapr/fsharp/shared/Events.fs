namespace Shared

open Domain.Events
open Giraffe
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks

[<AutoOpen>]
module Events =
    
    open Dapr.Client
    
    type SubscribeHttpHandler = DaprClient -> HttpHandler
    
    let publishDocRead (dapr: DaprClient) (event: DocReadEvent) =
        dapr.PublishEventAsync(DAPR_DOC_PUB_SUB, DAPR_TOPIC_DOC_READ, event)

    let subscribeDocRead (handler: DocReadEvent -> SubscribeHttpHandler) =
        DAPR_TOPIC_DOC_READ,
        fun dapr next ctx ->
            task {
                let! event = bindCloudEventDataAsync<DocReadEvent> ctx                
                return! handler event dapr next ctx
            }
