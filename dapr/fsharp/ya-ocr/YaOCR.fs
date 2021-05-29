module ReadFile

open Dapr.Client
open Saturn
open Giraffe
open FSharp.Control.Tasks
open Microsoft.AspNetCore.Http
open System.IO
open Shared

let fileRead =
    fun (dapr: DaprClient) (next: HttpFunc) (ctx: HttpContext) -> json {| ok = true |} next ctx

let routes dapr =
    router {
        get
            "/dapr/subscribe"
            (json (
                [ {| pubsubname = "pubsub"
                     topic = "file-read"
                     route = "file-read" |} ]
            ))

        post "/file-read" (fileRead dapr)
    }

let app =
    let dapr = DaprClientBuilder().Build()

    application {
        use_router (routes dapr)
        url (getAppUrl 5001)
        use_gzip
    }

[<EntryPoint>]
let main _ =
    run app
    0 // return an integer exit code
