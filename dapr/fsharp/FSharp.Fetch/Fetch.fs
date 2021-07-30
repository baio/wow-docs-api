module FSharp.Fetch

    open Oryx
    open Oryx.SystemTextJson.ResponseReader
    open System.Net.Http
    open System.Threading.Tasks
    open System.Text.Json

    type Method = POST | PUT | DELETE

    type Request<'t> = {
        Url: string
        Method: Method
        Body: 't option      
    }
    
    let private mapMethod =
        function
        | POST -> HttpHandler.POST
        | PUT -> HttpHandler.PUT
        | DELETE -> HttpHandler.DELETE

    let private mapBody  = function
        | Some body ->
            withContent (fun _ -> Json.JsonContent.Create body :> HttpContent)
        | None -> skip             

    let private jsonOptions = JsonSerializerOptions()

    let fetch<'t, 'r> (request: Request<'r>) = 

        let method = mapMethod request.Method


        let request = 
            method 
            >=> withUrl request.Url
            >=> mapBody request.Body
            >=> fetch
            >=> json<'t> jsonOptions

        let client = new HttpClient ()
        let ctx = 
            HttpContext.defaultContext     
            |> HttpContext.withHttpClient client 
        
        
        request |> runAsync ctx