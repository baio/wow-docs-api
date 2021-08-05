module ReadFile

open Giraffe
open FSharp.Control.Tasks
open Microsoft.AspNetCore.Http
open System.IO
open Shared
open Domain
open FSharp.Dapr

// TODO : Validate file size and image size

let fileUploadHandler =
    fun (env: DaprAppEnv) (next: HttpFunc) (ctx: HttpContext) ->
        task {
            match ctx.Request.HasFormContentType with
            | true ->
                let file =
                    ctx.Request.Form.Files
                    |> Seq.tryFind (fun f -> f.Name = "file")

                let docKey =
                    match ctx.Request.Form.TryGetValue "docKey" with
                    | true, v -> Some v.[0]
                    | _ -> None

                match file, docKey with
                | Some file, Some docId ->
                    let fileStream = file.OpenReadStream()
                    let memoryStream = new MemoryStream()
                    do! fileStream.CopyToAsync(memoryStream)
                    let bytes = memoryStream.ToArray()
                    let fileBase64 = System.Convert.ToBase64String bytes

                    let event =
                        { DocContent = fileBase64
                          DocKey = docId }

                    do! publishDocRead env event
                    return! json event next ctx
                | _ -> return! RequestErrors.BAD_REQUEST {| file = "Missed file with name file or docKey" |} next ctx
            | false -> return! RequestErrors.BAD_REQUEST {| file = "Not form content" |} next ctx
        }

let router =
    fun dapr ->
        route "/upload"
        >=> POST
        >=> (fileUploadHandler dapr)

[<EntryPoint>]
let main _ =
    runSharedDaprApp 5000 (DaprRouter router)
