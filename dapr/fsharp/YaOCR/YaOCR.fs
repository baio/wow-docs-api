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

//
let docRead (event: DocReadEvent) (env: DaprAppEnv) =

    task {

        let! words = extractWords true env event.DocContent

        let docExtractedText: DocExtarctedText =
            { Words = words
              Provider = DocTextExtarctedProvider.YaOCR }

        let docTextExtractedEvent: DocTextExtractedEvent =
            { DocKey = event.DocKey
              DocExtractedText = docExtractedText }

        do! publishDocTextExtracted env docTextExtractedEvent
        return true
    }


[<EntryPoint>]
let main _ =
    runSharedDaprApp 3001 (DaprSubs [ subscribeDocRead docRead ])
