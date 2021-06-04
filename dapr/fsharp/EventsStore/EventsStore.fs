module ReadFile

open Microsoft.Extensions.Logging
open Saturn
open FSharp.Control.Tasks
open Shared
open Domain
open FSharp.Dapr

//
type DocEntry =
    { DocId: string
      Store: DocStore option
      ExtractedText: DocExtarctedText option
      Label: DocLabeled option }

let createDocEntry docId =
    { DocId = docId
      Store = None
      ExtractedText = None
      Label = None }

let docRead (event: DocReadEvent) env =
    task {
        let docEntry = createDocEntry event.DocKey
        let! _ = tryCreateStateAsync env DAPR_DOC_STATE_STORE event.DocKey docEntry
        return true
    }

let docStored (event: DocStoredEvent) env =
    task {
        let! _ =
            tryUpdateOrCreateStateAsync
                env
                DAPR_DOC_STATE_STORE
                event.DocKey
                createDocEntry
                (fun doc -> { doc with Store = Some event.DocStore })

        return true
    }

let docTextExtracted (event: DocTextExtractedEvent) env =
    task {
        let! _ =
            tryUpdateOrCreateStateAsync
                env
                DAPR_DOC_STATE_STORE
                event.DocKey
                createDocEntry
                (fun doc ->
                    { doc with
                          ExtractedText = Some event.DocExtractedText })

        return true
    }

let docTextLabeled (event: DocLabeledEvent) env =
    task {
        let! _ =
            tryUpdateOrCreateStateAsync
                env
                DAPR_DOC_STATE_STORE
                event.DocKey
                createDocEntry
                (fun doc ->
                    { doc with
                          Label = Some event.DocLabeled })

        return true
    }

let subs =
    [ subscribeDocRead docRead
      subscribeDocStored docStored
      subscribeDocTextExtracted docTextExtracted
      subscribeDocLabeled docTextLabeled ]

[<EntryPoint>]
let main _ = runDaprApp 5002 (DaprSubs subs)
