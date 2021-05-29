module StoreDoc

open Dapr.Client
open Microsoft.Extensions.Logging
open Saturn
open Giraffe
open FSharp.Control.Tasks
open Microsoft.AspNetCore.Http
open Shared


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

let docRead =
    fun (dapr: DaprClient) (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let! doc = bindCloudEventDataAsync<DocRead> ctx            
            do! System.Threading.Tasks.Task.Delay(1000)
            let storedEvent: DocStored = 
                { 
                    DocKey = doc.DocKey; 
                    DocStore = { Url = "http://kek.com/123"; Provider = DocStoreProvider.YaCloud} 
                }
            do! dapr.PublishEventAsync(DAPR_DOC_PUB_SUB, DAPR_TOPIC_DOC_STORED, storedEvent)
            return! json storedEvent next ctx            
        }


let routes dapr =
    router {
        get
            "/dapr/subscribe"
            (json (
                [ {| pubsubname = DAPR_DOC_PUB_SUB
                     topic = DAPR_TOPIC_DOC_READ
                     route = DAPR_TOPIC_DOC_READ |}                                  
                ]
            ))

        post $"/{DAPR_TOPIC_DOC_READ}" (docRead dapr)
    }

let app = daprApp 5003 routes

[<EntryPoint>]
let main _ =
    run app
    0 // return an integer exit code

