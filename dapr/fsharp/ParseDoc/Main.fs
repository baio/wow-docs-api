module ParseDoc.Main


open FSharp.Control.Tasks
open Shared
open Domain
open FSharp.Dapr

let private handleDocTextExtracted (resources: Resources) (event: DocTextExtractedEvent) (env: DaprAppEnv) =

    let doc =
        match event.DocExtractedText.Result with
        | DocExtractedResult.Words words ->
            let parsed = parseDoc resources words env

            match parsed with
            | Some parsed -> parsed
            | None ->
                ErrorDoc
                    { Message = "Cant parse doc"
                      Code = -1 }
        | DocExtractedResult.Error err -> ErrorDoc err

    task {
        { DocParsed = { ParsedDoc = doc }
          DocKey = event.DocKey
          DocExtractedText = event.DocExtractedText }
        |> publishDocParsed env
        |> ignore

        return true
    }


[<EntryPoint>]
let main args =
    let resources = loadResources ()
    runSharedDaprApp 3030 (DaprSubs [ subscribeDocTextExtracted (handleDocTextExtracted resources) ]) args
