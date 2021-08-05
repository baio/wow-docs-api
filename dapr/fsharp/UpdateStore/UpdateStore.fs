module UpdateStore

open Microsoft.Extensions.Logging
open Saturn
open FSharp.Control.Tasks
open Shared
open Domain
open FSharp.Dapr

//

let TTL_FOR_STORE_PARSED_DOC = 60

type DocEntry =
    { DocKey: string
      DocParsed: DocParsed }

let docTextParsed (event: DocParsedEvent) env =
    creatStateTTLAsync
        env
        DAPR_DOC_STATE_STORE
        event.DocKey
        { DocKey = event.DocKey
          DocParsed = event.DocParsed }
        TTL_FOR_STORE_PARSED_DOC

let subs = [ subscribeDocParsed docTextParsed ]

[<EntryPoint>]
let main _ = runSharedDaprApp 3003 (DaprSubs subs)
