module UpdateStore

open Shared
open Domain
open FSharp.Dapr

let docTextParsed (event: DocParsedEvent) env =
    addDocParsed env event.DocKey event.DocParsed

let subs = [ subscribeDocParsed docTextParsed ]

[<EntryPoint>]
let main args =
    runSharedDaprApp 3003 (DaprSubs subs) args
