namespace FSharp.Dapr

open Dapr.Client
open Microsoft.Extensions.Logging

(*
type IDaprAppEnv =
    abstract member GetLogger : unit -> ILogger
    abstract member GetDapr : unit -> DaprClient
*)

type DaprAppEnv = { Logger: ILogger; Dapr: DaprClient }
(*
    interface IDaprAppEnv with
        member __.GetLogger() = __.Logger
        member __.GetDapr() = __.Dapr
*)
