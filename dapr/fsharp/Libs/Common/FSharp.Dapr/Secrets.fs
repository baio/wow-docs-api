namespace FSharp.Dapr

[<AutoOpen>]
module Secrets =
    open Dapr.Client
    open FSharp.Control.Tasks

    let private dictToSeq (dic: System.Collections.Generic.IDictionary<_, _>) = dic |> Seq.map (|KeyValue|)

    let private getSecrets (dapr: DaprClient) secretStoreName secretKey =
        dapr.GetSecretAsync(storeName = secretStoreName, key = secretKey)

    let private getAllSecretsFlatted (dapr: DaprClient) secretStoreName secretKey names =
        let secretTasks =
            names
            |> Seq.map
                (fun name ->
                    let path = $"{secretKey}:{name}"

                    getSecrets dapr secretStoreName path
                    |> Async.AwaitTask)

        task {
            let! result = secretTasks |> Async.Parallel |> Async.StartAsTask

            return
                result
                |> Seq.fold
                    (fun acc v ->
                        let s = v |> dictToSeq |> Seq.tryHead

                        match s with
                        | Some (k, v) -> (k.Split(':').[1], v) :: acc
                        | _ -> acc)
                    (List.empty)
                |> dict
        }


    let private getAllSecretsSafe (dapr: DaprClient) secretStoreName secretKey names =
        task {
            try
                let! res = getSecrets dapr secretStoreName secretKey
                return res :> System.Collections.Generic.IDictionary<_, _>
            with
            | :? Dapr.DaprException ->
                // due to inconsistency between k8s and localfile secret stores
                return! getAllSecretsFlatted dapr secretStoreName secretKey names
        }

    let getAllSecrets (dapr: DaprClient) secretStoreName secretKey names =
        task {

            let! secrets = getAllSecretsSafe dapr secretStoreName secretKey names

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
