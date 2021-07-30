namespace FSharp.Dapr

[<AutoOpen>]
module Secrets =
    open Dapr.Client
    open FSharp.Control.Tasks

    let getSecret' (dapr: DaprClient) secretStoreName secretKey =         
        dapr.GetSecretAsync(storeName = secretStoreName, key = secretKey)
        

    let getSecret (dapr: DaprClient) secretStoreName secretKey =         
        task {
            let! result = getSecret' dapr secretStoreName secretKey
            let firstResult = result.Values |> Seq.tryHead
            return firstResult            
        }

    let getSecrets (dapr: DaprClient) secretStoreName secretKeys =         
        secretKeys
        |> Seq.map (
            getSecret dapr secretStoreName >> Async.AwaitTask
        )
        |> Async.Parallel

    let getAllSecrets (dapr: DaprClient) secretStoreName secretKeys =
        task {
            let! secrets = getSecrets dapr secretStoreName secretKeys
            let result =
                secrets |> Seq.fold(fun acc v ->
                    match acc, v with
                    | (Some acc), (Some v) -> 
                        Some(v::acc)
                    | _ -> 
                        None 
                ) (Some [])
            return result                
        }         
