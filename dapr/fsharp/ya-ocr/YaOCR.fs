module StoreDoc

open Dapr.Client
open Microsoft.Extensions.Logging
open Saturn
open Giraffe
open FSharp.Control.Tasks
open Microsoft.AspNetCore.Http
open Shared
open Domain

//

let docRead =
    fun (dapr: DaprClient) (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let! doc = bindCloudEventDataAsync<DocTextExtractedEvent> ctx            
            do! System.Threading.Tasks.Task.Delay(1000)
            let storedEvent: DocLabeledEvent = 
                { 
                    DocKey = doc.DocKey; 
                    DocLabeled = { Label = Passport; Provider = DocLabeledProvider.CustomPy} 
                }
            do! dapr.PublishEventAsync(DAPR_DOC_PUB_SUB, DAPR_TOPIC_DOC_LABELED, storedEvent)
            return! json storedEvent next ctx            
        }


let subs = [(DAPR_TOPIC_DOC_TEXT_EXTRACTED, docRead)]

let app = daprApp 5003 (DaprSubs (DAPR_DOC_PUB_SUB, subs))

[<EntryPoint>]
let main _ =
    run app
    0 // return an integer exit code

