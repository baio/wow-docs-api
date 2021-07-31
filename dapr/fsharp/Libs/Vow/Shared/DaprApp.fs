namespace Shared

[<AutoOpen>]
module DaprApp =

    open Saturn
    open FSharp.Dapr

    let sharedDaprApp = daprApp createSerilogLogger

    let runDaprApp port app =
        let app = sharedDaprApp port app
        run app
        0
