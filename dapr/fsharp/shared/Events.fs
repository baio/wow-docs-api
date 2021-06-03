namespace Shared

open System.Threading.Tasks
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
        
    
    let subscribeDapr'<'e> (topicName: string) (handler: 'e -> SubscribeHttpHandler) =
        topicName,
        fun dapr next ctx ->
            task {
                let! event = bindCloudEventDataAsync<'e> ctx
                return! handler event dapr next ctx
            }
            
    let subscribeDapr<'e, 'r> topicName (handler: 'e -> DaprClient -> Task<'r>) =
        subscribeDapr'<'e>
            topicName
            (fun event dapr next ctx ->
                task {
                    let! resultEvent = handler event dapr
                    return! json resultEvent next ctx
                })
            
    let subscribeDocRead x = x |> subscribeDapr<DocReadEvent, bool> DAPR_TOPIC_DOC_READ
    
    (*       
    let subscribeDocRead' (handler: DocReadEvent -> SubscribeHttpHandler) =
        DAPR_TOPIC_DOC_READ,
        fun dapr next ctx ->
            task {
                let! event = bindCloudEventDataAsync<DocReadEvent> ctx
                return! handler event dapr next ctx
            }

    let subscribeDocRead<'r> (handler: DocReadEvent -> DaprClient -> Task<'r>) =
        subscribeDocRead'
            (fun event dapr next ctx ->
                task {
                    let! resultEvent = handler event dapr
                    return! json resultEvent next ctx
                })
    *)                
