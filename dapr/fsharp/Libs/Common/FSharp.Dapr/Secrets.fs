namespace FSharp.Dapr

[<AutoOpen>]
module Secrets =
    open Dapr.Client
    open FSharp.Control.Tasks

    let getSecrets (dapr: DaprClient) secretStoreName secretKey =
        dapr.GetSecretAsync(storeName = secretStoreName, key = secretKey)

    let getAllSecrets (dapr: DaprClient) secretStoreName secretKey names =
        task {
            let! secrets = getSecrets dapr secretStoreName secretKey

            let result =
                names
                |> Seq.fold
                    (fun acc name ->
                        let secret = secrets.TryGetValue name

                        match secret with
                        | (true, secretValue) ->
                            let acc =
                                match acc with
                                | Some acc -> acc
                                | None -> []

                            Some(acc @ [ secretValue ])
                        | _ -> None)
                    None

            return result
        }

    let getSecret (dapr: DaprClient) secretStoreName secretKey name =
        task {
            let! secrets = getAllSecrets dapr secretStoreName secretKey [ name ]

            match secrets with
            | Some [ secret ] -> return (Some secret)
            | _ -> return None

        }
