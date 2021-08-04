namespace Shared

[<AutoOpen>]
module DaprApp =

    open FSharp.Dapr

    //let runSharedDaprApp = runDaprApp createSerilogLogger

    let runSharedDaprApp = runDaprApp createSerilogLogger
