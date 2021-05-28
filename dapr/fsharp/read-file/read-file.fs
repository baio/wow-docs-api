module ReadFile

open Dapr.Client
open Microsoft.Extensions.Configuration
open Saturn
open Giraffe
open FSharp.Control.Tasks
open Microsoft.AspNetCore.Http
open System.IO
open Shared
open Microsoft.Extensions.Logging

let fileUploadHandler =
    fun (dapr: DaprClient) (next: HttpFunc) (ctx: HttpContext) ->
        let logger = ctx.GetLogger()
        logger.LogInformation("test {ver}", 1)

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
                    let result = {| FileBase64 = fileBase64 |}
                    do! dapr.PublishEventAsync("pubsub", "file-read", result)
                    return! json result next ctx
                | None -> return! RequestErrors.BAD_REQUEST {| file = "Missed file with name file" |} next ctx
            | false -> return! RequestErrors.BAD_REQUEST {| file = "Not form content" |} next ctx
        }

let app  =
    let dapr = DaprClientBuilder().Build()
    
    let config =
        ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build()
            
    printfn "++++ %s" (config.Item("ElasticHost"))            

    application {
        use_config(fun _ -> config)
        use_router (router { post "/upload" (fileUploadHandler dapr) })
        url (getAppUrl 5000)
        use_gzip
        webhost_config (createSerilogLogger config)
    } 

[<EntryPoint>]
let main args =
    run app 
    0 // return an integer exit code
