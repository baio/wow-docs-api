namespace Shared

open System.Threading.Tasks
open Domain.Events
open Giraffe
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks

[<AutoOpen>]
module Events =

    open Dapr.Client

    type SubscribeHttpHandler<'x> = 'x -> HttpHandler

    let publishDocRead (dapr: DaprClient) (event: DocReadEvent) =
        dapr.PublishEventAsync(DAPR_DOC_PUB_SUB, DAPR_TOPIC_DOC_READ, event)
        
    
    let subscribeDapr'<'x, 'e> (topicName: string) (handler: 'e -> SubscribeHttpHandler<'x>) =
        topicName,
        fun dapr next ctx ->
            task {
                let! event = bindCloudEventDataAsync<'e> ctx
                return! handler event dapr next ctx
            }
            
    let subscribeDapr<'x, 'e, 'r> topicName (handler: 'e -> 'x -> Task<'r>) =
        subscribeDapr'<'x, 'e>
            topicName
            (fun event dapr next ctx ->
                task {
                    let! resultEvent = handler event dapr
                    return! json resultEvent next ctx
                })
            
    let subscribeDocRead x = x |> subscribeDapr<DaprAppEnv, DocReadEvent, bool> DAPR_TOPIC_DOC_READ
    
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
