module ReadFile

open Dapr.Client
open Microsoft.Extensions.Configuration
open Saturn
open Giraffe
open FSharp.Control.Tasks
open Microsoft.AspNetCore.Http
open System.IO
open Shared
open Domain

let fileUploadHandler =
    fun (env: DaprAppEnv) (next: HttpFunc) (ctx: HttpContext) ->
        task {
            match ctx.Request.HasFormContentType with
            | true ->
                let file =
                    ctx.Request.Form.Files
                    |> Seq.tryFind (fun f -> f.Name = "file")

                match file with
                | Some file ->
                    let fileStream = file.OpenReadStream()
                    let memoryStream = new MemoryStream()
                    do! fileStream.CopyToAsync(memoryStream)
                    let bytes = memoryStream.ToArray()
                    let fileBase64 = System.Convert.ToBase64String bytes
                    let event = { DocContent = "xxx"; DocKey = fileBase64.Substring(0, System.Random(100).Next(1, 300)) }
                    do! publishDocRead env.Dapr event
                    return! json event next ctx
                | None -> return! RequestErrors.BAD_REQUEST {| file = "Missed file with name file" |} next ctx
            | false -> return! RequestErrors.BAD_REQUEST {| file = "Not form content" |} next ctx
        }

let router = fun dapr -> router { post "/upload" (fileUploadHandler dapr) }

let app = daprApp 3000 (DaprRouter router)

[<EntryPoint>]
let main _ =
    run app 
    0 // return an integer exit code
