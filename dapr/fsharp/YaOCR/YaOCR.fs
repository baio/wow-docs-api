module YaOCR.Main

open FSharp.Control.Tasks
open Shared
open Domain
open FSharp.Dapr
open YaOCR.IamToken
open YaOCR.ExtractText

let rec private extractWords retryOnUanauthorized (env: DaprAppEnv) imgBase64 =
    task {
        let! iamToken = getIAMToken env

        let! result = extractText env.Dapr iamToken imgBase64

        let! words =
            match result with
            | Ok words -> System.Threading.Tasks.Task.FromResult words
            | Error ex ->
                match ex with
                | :? UnAuthorizedException ->
                    logWarn env.Logger "YaOcr unauthorized error consider IAM token expired or revoked"

                    task {
                        match retryOnUanauthorized with
                        | true ->
                            logDebug env.Logger "Retry extract text with another IAM token"
                            do! clearIAMTokenCache env
                            return! extractWords false env imgBase64
                        | false ->
                            logError1 env.Logger "Unauthorized {error} on retry" ex
                            return raise ex
                    }
                | _ ->
                    logError1 env.Logger "YaOCR error {error}" ex
                    raise ex

        return words
    }

let private getTextExtractedEvent dokcKey result provider =
    let docExtractedText: DocExtarctedText = { Result = result; Provider = provider }

    { DocKey = dokcKey
      DocExtractedText = docExtractedText }

let docRead (event: DocReadEvent) (env: DaprAppEnv) =
    task {

        try
            let! words = extractWords true env event.DocContent

            let docTextExtractedEvent =
                getTextExtractedEvent event.DocKey (DocExtractedResult.Words words) YaOCR

            do! publishDocTextExtracted env docTextExtractedEvent
            return true
        with
        | ex ->
            let docTextExtractedEvent =
                getTextExtractedEvent
                    event.DocKey
                    (DocExtractedResult.Error(
                        { Message = ex.Message
                          Code = ex.HResult }
                    ))
                    YaOCR

            do! publishDocTextExtracted env docTextExtractedEvent
            return false
    }


[<EntryPoint>]
let main args =
    runSharedDaprApp 3002 (DaprSubs [ subscribeDocRead docRead ]) args
