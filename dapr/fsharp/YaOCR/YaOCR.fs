module StoreDoc

open FSharp.Control.Tasks
open Shared
open Domain
open FSharp.Dapr

//
let docRead (event: DocReadEvent) { Dapr = dapr } =
    task {
        do! System.Threading.Tasks.Task.Delay(2000)

        let docExtractedText : DocExtarctedText =
            { Words = event.DocContent.Split("a")
              Provider = DocTextExtarctedProvider.YaOCR }

        let docTextExtractedEvent : DocTextExtractedEvent =
            { DocKey = event.DocKey
              DocExtractedText = docExtractedText }

        do! publishDocTextExtracted dapr docTextExtractedEvent
        return true
    }


[<EntryPoint>]
let main _ =
    runDaprApp 5003 (DaprSubs [ subscribeDocRead docRead ])
