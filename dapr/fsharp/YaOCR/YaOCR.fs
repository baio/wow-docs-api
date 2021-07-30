module StoreDoc

open FSharp.Control.Tasks
open Shared
open Domain
open FSharp.Dapr
open YaAuth


let SECRET_STORE_NAME = "vow-docs"
let SECRET_NAME = "vow-docs-ya"
let YA_SERVICE_ACCOUNT_ID_KEY = "YA_SERVICE_ACCOUNT_ID"
let YA_KEY_ID_KEY = "YA_KEY_ID"
let YA_PRIVATE_KEY_KEY = "ya-private-key.pem"

let STORE_IAM_KEY = "ya-iam-key"

let private getYaConfig dapr =

    task {

        let! secrets = 
            getAllSecrets 
                dapr
                SECRET_STORE_NAME 
                SECRET_NAME
                [YA_SERVICE_ACCOUNT_ID_KEY; YA_KEY_ID_KEY; YA_PRIVATE_KEY_KEY]        

        let (yaServiceAccountId, yaKeyId, yaPrivateKey) =
            match secrets with
            | Some ([yaServiceAccountId; yaKeyId; yaPrivateKey]) -> yaServiceAccountId, yaKeyId, yaPrivateKey
            | _ ->
                raise (exn "Some Ya secrets not found")

        let config = {
            ServiceAccountId = yaServiceAccountId
            KeyId = yaKeyId
            PrivateKey = yaPrivateKey
        }

        return config
    }


let private requestIAMToken env = 

    task {

        let! config = getYaConfig env.Dapr

        let! iamResult = getIAMToken config 

        match iamResult with
        | Ok (token, ttl) -> 
            do! createStateWithMetadataAsync  
                    env 
                    DAPR_DOC_STATE_STORE 
                    STORE_IAM_KEY 
                    token 
                    (readOnlyDict ["ttlInSeconds", (ttl - 600).ToString()])
            return token                
        | Error ex ->
            return raise ex    
    }

let private getIAMToken env = 

    task {

        let! existentIamKey = getStateAsync env DAPR_DOC_STATE_STORE STORE_IAM_KEY

        match existentIamKey with
        | None ->             
            logTrace env.Logger "Ya IAM token not found in cache, request new one"
            return! requestIAMToken env
        | Some  existentIamKey ->
            logTrace env.Logger "Ya IAM token found in cache"
            return existentIamKey
    }

//
let docRead (event: DocReadEvent) (env: DaprAppEnv) =

    task {

        let! iamToken = getIAMToken env

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
