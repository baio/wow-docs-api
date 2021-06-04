namespace FSharp.Dapr

open Dapr.Client
open Microsoft.Extensions.Logging

type DaprAppEnv = { Logger: ILogger; Dapr: DaprClient }

