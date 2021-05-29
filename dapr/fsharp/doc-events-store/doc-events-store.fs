module ReadFile

open Dapr.Client
open Microsoft.Extensions.Logging
open Saturn
open Giraffe
open FSharp.Control.Tasks
open Microsoft.AspNetCore.Http
open Shared
open Domain


//
type DocEntity = {
    DocId: string
    Store: DocStore option
}

let createDocEntry docId =
    {
        DocId = docId
        Store = None
    }
    

let docRead =
    fun (dapr: DaprClient) (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let logger = ctx.GetLogger()
            //let! body = ctx.ReadBodyFromRequestAsync()
            //printfn "333 %s" body
            let! data = bindCloudEventDataAsync<DocRead> ctx
            let docEntry = createDocEntry data.DocKey
            let! res = tryCreateStateAsync dapr DAPR_DOC_STATE_STORE data.DocKey docEntry
            match res with
            | true -> logger.LogDebug("{statestore} updated with new {document}", "statestore", docEntry)
            | false -> logger.LogDebug("{statestore} failed to update, {document} already exists", "statestore", docEntry)
            return! json res next ctx            
        }

let docStored =
    fun (dapr: DaprClient) (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let logger = ctx.GetLogger()            
            let! data = bindCloudEventDataAsync<DocStored> ctx
            let! res = 
                tryUpdateOrCreateStateAsync 
                    dapr 
                    DAPR_DOC_STATE_STORE
                    data.DocKey 
                    (fun id -> createDocEntry id) 
                    (fun doc -> { doc with Store = Some data.DocStore })

            match res.IsSuccess with
            | true -> logger.LogDebug("{statestore} document with {documentId} is updated with {result}", "statestore", data.DocKey, res)
            | false -> logger.LogWarning("{statestore} document with {documentId} fail to update with {result}", "statestore", data.DocKey, res)
                        
            return! json res next ctx
            
        }

let routes dapr =
    router {
        get
            "/dapr/subscribe"
            (json (
                [ {| pubsubname = DAPR_DOC_PUB_SUB
                     topic = DAPR_TOPIC_DOC_READ
                     route = DAPR_TOPIC_DOC_READ |}
                  {| 
                    pubsubname = DAPR_DOC_PUB_SUB
                    topic = DAPR_TOPIC_DOC_STORED
                    route = DAPR_TOPIC_DOC_STORED |}                                          
                ]
            ))

        post "/doc-read" (docRead dapr)
        post "/doc-stored" (docStored dapr)
    }

let app = daprApp 5002 routes

[<EntryPoint>]
let main _ =
    run app
    0 // return an integer exit code

