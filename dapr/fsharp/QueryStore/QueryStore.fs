module ReadFile

open Giraffe
open FSharp.Control.Tasks
open Microsoft.AspNetCore.Http
open System.IO
open Shared
open Domain
open FSharp.Dapr

let getParsedDoc =
    fun (env: DaprAppEnv) (docKey: string) (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let! result = getDocParsed env docKey

            match result with
            | None -> return! RequestErrors.NOT_FOUND {| message = $"Item with key [{docKey}] not found" |} next ctx
            | Some result -> return! json result next ctx
        }

let router =
    fun dapr ->
        GET
        >=> routef "/docs/%s/parsed" (getParsedDoc dapr)

[<EntryPoint>]
let main _ =
    runSharedDaprApp 3004 (DaprRouter router)
