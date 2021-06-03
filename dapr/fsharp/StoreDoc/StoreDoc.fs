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
    fun (event: DocReadEvent) (env: DaprAppEnv) ->
        task {
            do! System.Threading.Tasks.Task.Delay(1000)

            let storedEvent : DocStoredEvent =
                { DocKey = event.DocKey
                  DocStore =
                      { Url = "http://kek.com/123"
                        Provider = DocStoreProvider.YaCloud } }

            do! publishDocStored env.Dapr storedEvent
            return true
        }

let subs = [ subscribeDocRead docRead ]

let app = daprApp 5003 (DaprSubs subs)

[<EntryPoint>]
let main _ =
    run app
    0 // return an integer exit code
