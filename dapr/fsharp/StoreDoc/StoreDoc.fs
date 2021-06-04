module StoreDoc

open Saturn
open FSharp.Control.Tasks
open Shared
open Domain
open FSharp.Dapr

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

            do! publishDocStored env storedEvent
            return true
        }

let subs = [ subscribeDocRead docRead ]

[<EntryPoint>]
let main _ = runDaprApp 5003 (DaprSubs subs)
