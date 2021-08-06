module internal YaOCR.Config

open FSharp.Control.Tasks
open FSharp.Dapr
open YaAuth
open YaOCR.Constants

let getYaConfig dapr =

    task {

        let! secrets =
            getAllSecrets
                dapr
                SECRET_STORE_NAME
                SECRET_NAME
                [ YA_SERVICE_ACCOUNT_ID_KEY
                  YA_KEY_ID_KEY
                  YA_PRIVATE_KEY_KEY ]

        let (yaServiceAccountId, yaKeyId, yaPrivateKey) =
            match secrets with
            | Some ([ yaServiceAccountId; yaKeyId; yaPrivateKey ]) -> yaServiceAccountId, yaKeyId, yaPrivateKey
            | _ -> raise (exn "Some Ya secrets not found")

        let config =
            { ServiceAccountId = yaServiceAccountId
              KeyId = yaKeyId
              PrivateKey = yaPrivateKey }

        printfn "111 %O" config               

        return config
    }
