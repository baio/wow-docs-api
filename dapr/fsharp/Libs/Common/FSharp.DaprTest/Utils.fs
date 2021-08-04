module FSharp.DaprTest.Utils

open FSharp.Control.Tasks
open Microsoft.AspNetCore.TestHost
open Microsoft.AspNetCore.Hosting
open System.Net.Http

open FSharp.Dapr
open Giraffe

let private getTestHost webApp =
    WebHostBuilder().UseTestServer()
    |> configureWebHostBuilder webApp



let testRequest handler (request: HttpRequestMessage) =

    let resp =
        task {
            use server = new TestServer(getTestHost handler)
            use client = server.CreateClient()
            let! response = request |> client.SendAsync
            return response
        }

    resp.Result
