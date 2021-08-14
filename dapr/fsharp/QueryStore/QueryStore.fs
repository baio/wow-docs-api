module QueryStore.Main

open Giraffe
open FSharp.Control.Tasks
open Microsoft.AspNetCore.Http
open Shared
open Domain
open FSharp.Dapr
open QueryStore.Formatted

let getParsedDoc =
    fun (env: DaprAppEnv) (docKey: string) (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let! result = getDocParsed env docKey

            match result with
            | None -> return! RequestErrors.NOT_FOUND {| message = $"Item with key [{docKey}] not found" |} next ctx
            | Some result ->
                match result.ParsedDoc with
                | ErrorDoc _ ->
                    return!
                        RequestErrors.UNPROCESSABLE_ENTITY {| message = $"Cant extract text from [{docKey}]" |} next ctx
                | _ ->
                    let formatOpt = ctx.TryGetQueryStringValue("format")

                    let doc =
                        match formatOpt with
                        | Some _ -> formatDoc result.ParsedDoc
                        | None -> result.ParsedDoc :> obj

                    return! json doc next ctx
        }

let router =
    fun dapr ->
        GET
        >=> routef "/docs/%s/parsed" (getParsedDoc dapr)

[<EntryPoint>]
let main args =
    runSharedDaprApp 3004 (DaprRouter router) args
