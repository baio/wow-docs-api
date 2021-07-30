namespace FSharp.Dapr

[<AutoOpen>]
module Secrets =
    open Dapr.Client
    open FSharp.Control.Tasks

    let getSecrets (dapr: DaprClient) secretStoreName secretKey =         
        dapr.GetSecretAsync(storeName = secretStoreName, key = secretKey)
        
    (*
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
    *)        

    let getAllSecrets (dapr: DaprClient) secretStoreName secretKey names =
        task {
            let! secrets = getSecrets dapr secretStoreName secretKey
            let result =
                names |> Seq.fold(fun acc name ->
                    let secret = secrets.TryGetValue name
                    match secret with
                    | (true, secretValue) -> 
                        let acc = 
                            match acc with
                            | Some acc -> acc
                            | None -> []
                        Some(acc@[secretValue])
                    | _ -> 
                        None 
                ) None
            return result                
        }         
