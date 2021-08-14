module ParseDoc.Main


open FSharp.Control.Tasks
open Shared
open Domain
open FSharp.Dapr

let private handleDocTextExtracted (resources: Resources) (event: DocTextExtractedEvent) (env: DaprAppEnv) =

    let doc =
        match event.DocExtractedText.Result with
        | DocExtractedResult.Words words -> parseDoc resources words env
        | DocExtractedResult.Error err -> Some(ErrorDoc err)

    task {
        match doc with
        | Some doc ->
            { DocParsed = { ParsedDoc = doc }
              DocKey = event.DocKey
              DocExtractedText = event.DocExtractedText }
            |> publishDocParsed env
            |> ignore

            return true
        | _ -> return false
    }


[<EntryPoint>]
let main args =
    let resources = loadResources ()
    runSharedDaprApp 3030 (DaprSubs [ subscribeDocTextExtracted (handleDocTextExtracted resources) ]) args
