module ReadFile

open Dapr.Client
open Microsoft.Extensions.Logging
open Saturn
open Giraffe
open FSharp.Control.Tasks
open Microsoft.AspNetCore.Http
open Shared


type CloudEvent<'a> =
    { Id: string
      SpecVersion: string
      Source: string
      Type: string
      DataContentType: string
      PubSubName: string
      TraceId: string
      Topic: string
      DataSchema: string option
      Subject: string option
      Time: string option
      Data: 'a
      DataBase64: string option }

//
type DocStoreProvider =
    | YaCloud

[<CLIMutable>]
type DocStore = {
    Url: string
    Provider: DocStoreProvider
}

//
type DocRead = { DocKey: string; DocContent: string; }
[<CLIMutable>]
type DocStored = { DocKey: string; DocStore: DocStore; }

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
            let! event = ctx.BindJsonAsync<CloudEvent<DocRead>>()
            let data = event.Data

            let docEntry = createDocEntry data.DocKey

            // TODO : tryCreateStateAsync
            let! res = dapr.TrySaveStateAsync("statestore", docEntry.DocId, docEntry, "-1")
            match res with
            | true -> logger.LogDebug("{statestore} updated with new {document}", "statestore", docEntry)
            | false -> logger.LogDebug("{statestore} failed to update, {document} already exists", "statestore", docEntry)
            return! json docEntry next ctx
            
        }

let docStored =
    fun (dapr: DaprClient) (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let logger = ctx.GetLogger()
            logger.LogInformation("+++")
            let! event = ctx.BindJsonAsync<CloudEvent<DocStored>>()
            printfn "111 %O" event
            let data = event.Data

            // TODO : tryGetOrCreateState
            let! docEntry = dapr.GetStateEntryAsync<DocEntity>("statestore", data.DocKey)
            let (etag, doc) =
                match box docEntry.Value with
                | null -> 
                    // document still not created
                    ("-1", createDocEntry data.DocKey)
                | _  -> (docEntry.ETag, docEntry.Value)
                        
            let doc = { doc with Store = Some data.DocStore; }
            printfn "2222 %s %O" etag doc
            let! res = dapr.TrySaveStateAsync("statestore", doc.DocId, doc, etag)
            match res with
            | true -> logger.LogDebug("{statestore} updated with new {document}", "statestore", docEntry)
            | false -> logger.LogWarning("{statestore} failed to update doument {doc}, wrong {etag}", "statestore", doc, etag)
                        
            return! json {| OK = true |} next ctx
            
        }

let routes dapr =
    router {
        get
            "/dapr/subscribe"
            (json (
                [ {| pubsubname = "pubsub"
                     topic = "doc-read"
                     route = "doc-read" |}
                  {| 
                    pubsubname = "pubsub"
                    topic = "doc-stored"
                    route = "doc-stored" |}                                          
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

