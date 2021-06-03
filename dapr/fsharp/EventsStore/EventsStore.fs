module ReadFile

open Microsoft.Extensions.Logging
open Saturn
open FSharp.Control.Tasks
open Shared
open Domain
open FSharp.Dapr

//
type DocEntity =
    { DocId: string
      Store: DocStore option
      ExtractedText: DocExtarctedText option
      Label: DocLabeled option }

let createDocEntry docId =
    { DocId = docId
      Store = None
      ExtractedText = None
      Label = None }

let docRead (event: DocReadEvent) { Logger = logger; Dapr = dapr } =
    task {
        let docEntry = createDocEntry event.DocKey
        let! res = tryCreateStateAsync dapr DAPR_DOC_STATE_STORE event.DocKey docEntry

        match res with
        | true -> logger.LogDebug("{statestore} updated with new {document}", "statestore", docEntry)
        | false -> logger.LogDebug("{statestore} failed to update, {document} already exists", "statestore", docEntry)

        return true
    }

let docStored (event: DocStoredEvent) { Logger = logger; Dapr = dapr } =
    task {
        let! res =
            tryUpdateOrCreateStateAsync
                dapr
                DAPR_DOC_STATE_STORE
                event.DocKey
                createDocEntry
                (fun doc -> { doc with Store = Some event.DocStore })

        match res.IsSuccess with
        | true ->
            logger.LogDebug(
                "{statestore} document with {documentId} is updated with {result}",
                "statestore",
                event.DocKey,
                res
            )
        | false ->
            logger.LogWarning(
                "{statestore} document with {documentId} fail to update with {result}",
                "statestore",
                event.DocKey,
                res
            )

        return true
    }

let docTextExtracted (event: DocTextExtractedEvent) { Logger = logger; Dapr = dapr } =
    task {
        let! res =
            tryUpdateOrCreateStateAsync
                dapr
                DAPR_DOC_STATE_STORE
                event.DocKey
                createDocEntry
                (fun doc ->
                    { doc with
                          ExtractedText = Some event.DocExtractedText })

        match res.IsSuccess with
        | true ->
            logger.LogDebug(
                "{statestore} document with {documentId} is updated with {result}",
                "statestore",
                event.DocKey,
                res
            )
        | false ->
            logger.LogWarning(
                "{statestore} document with {documentId} fail to update with {result}",
                "statestore",
                event.DocKey,
                res
            )

        return true
    }

let docTextLabeled (event: DocLabeledEvent) { Logger = logger; Dapr = dapr } =
    task {
        let! res =
            tryUpdateOrCreateStateAsync
                dapr
                DAPR_DOC_STATE_STORE
                event.DocKey
                createDocEntry
                (fun doc ->
                    { doc with
                          Label = Some event.DocLabeled })

        match res.IsSuccess with
        | true ->
            logger.LogDebug(
                "{statestore} document with {documentId} is updated with {result}",
                "statestore",
                event.DocKey,
                res
            )
        | false ->
            logger.LogWarning(
                "{statestore} document with {documentId} fail to update with {result}",
                "statestore",
                event.DocKey,
                res
            )

        return true
    }

let subs =
    [ subscribeDocRead docRead
      subscribeDocStored docStored
      subscribeDocTextExtracted docTextExtracted
      subscribeDocLabeled docTextLabeled ]

[<EntryPoint>]
let main _ = runDaprApp 5002 (DaprSubs subs)
