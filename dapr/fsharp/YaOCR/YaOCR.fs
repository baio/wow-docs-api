module StoreDoc

open FSharp.Control.Tasks
open Shared
open Domain
open FSharp.Dapr
open YaAuth


let YA_SECRET_STORE_NAME = "kubernetes"
let YA_SERVICE_ACCOUNT_ID = "YA_SERVICE_ACCOUNT_ID"
let YA_KEY_ID = "YA_KEY_ID"
let YA_PRIVATE_KEY = "YA_PRIVATE_KEY"

let private getYaConfig dapr =

    task {

        let! secrets = 
            getAllSecrets 
                dapr
                YA_SECRET_STORE_NAME 
                [YA_SERVICE_ACCOUNT_ID; YA_KEY_ID; YA_PRIVATE_KEY]        

        let (yaServiceAccountId, yaKeyId, yaPrivateKey) =
            match secrets with
            | Some ([yaServiceAccountId; yaKeyId; yaPrivateKey]) -> yaServiceAccountId, yaKeyId, yaPrivateKey
            | _ ->
                raise (exn "Some Ya secrets not found")

        printfn "+++ %s" yaServiceAccountId                

        let config = {
            ServiceAccountId = yaServiceAccountId
            KeyId = yaKeyId
            PrivateKey = yaPrivateKey
        }

        return config
    }


let private getIAMToken dapr = 

    task {

        let! config = getYaConfig dapr

        let! iamResult = getIAMToken config 

        match iamResult with
        | Ok token -> 
            return token                
        | Error ex ->
            return raise ex    
    }

//
let docRead (event: DocReadEvent) (env: DaprAppEnv) =

    task {

        let! iamToken = getIAMToken env.Dapr

        printfn "+++ %s" iamToken

        let docExtractedText : DocExtarctedText =
            { Words = event.DocContent.Split("a")
              Provider = DocTextExtarctedProvider.YaOCR }

        let docTextExtractedEvent : DocTextExtractedEvent =
            { DocKey = event.DocKey
              DocExtractedText = docExtractedText }

        do! publishDocTextExtracted env docTextExtractedEvent
        return true
    }


[<EntryPoint>]
let main _ =
    runDaprApp 3001 (DaprSubs [ subscribeDocRead docRead ])
