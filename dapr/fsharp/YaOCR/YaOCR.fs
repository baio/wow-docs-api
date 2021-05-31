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
            let! doc = bindCloudEventDataAsync<DocReadEvent> ctx
            do! System.Threading.Tasks.Task.Delay(2000)

            let docExtractedText : DocExtarctedText =
                { Words = doc.DocContent.Split("a")
                  Provider = DocTextExtarctedProvider.YaOCR }

            let docTextExtractedEvent : DocTextExtractedEvent =
                { DocKey = doc.DocKey
                  DocExtractedText = docExtractedText }

            do! dapr.PublishEventAsync(DAPR_DOC_PUB_SUB, DAPR_TOPIC_DOC_TEXT_EXTRACTED, docTextExtractedEvent)
            return! json docExtractedText next ctx
        }


let subs = [ (DAPR_TOPIC_DOC_READ, docRead) ]

let app =
    daprApp 5003 (DaprSubs(DAPR_DOC_PUB_SUB, subs))

[<EntryPoint>]
let main _ =
    run app
    0 // return an integer exit code
