module internal YaOCR.IamToken

open FSharp.Control.Tasks
open Shared
open FSharp.Dapr
open YaAuth
open Config

let STORE_IAM_KEY = "ya-iam-key"

let private cacheIamToken env token ttl =
    creatStateTTLAsync env DAPR_DOC_STATE_STORE STORE_IAM_KEY token ttl

let private getCachedIamToken env =
    getStateAsync env DAPR_DOC_STATE_STORE STORE_IAM_KEY

let private requestIAMToken env =

    task {

        let! config = getYaConfig env.Dapr

        let! iamResult = getIAMToken config

        match iamResult with
        | Ok (token, ttl) ->
            do! cacheIamToken env token ttl
            return token
        | Error ex -> return raise ex
    }

let clearIAMTokenCache env =
    deleteStateAsync env DAPR_DOC_STATE_STORE STORE_IAM_KEY


let getIAMToken env =

    task {

        let! cachedIamToken = getCachedIamToken env

        match cachedIamToken with
        | None ->
            logTrace env.Logger "Ya IAM token not found in cache, request new one"
            return! requestIAMToken env
        | Some cachedIamToken ->
            logTrace env.Logger "Ya IAM token found in cache"
            return cachedIamToken
    }
