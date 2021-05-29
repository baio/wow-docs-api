namespace Shared

[<AutoOpen>]
module CloudEvent = 

    open Microsoft.AspNetCore.Http
    open Giraffe
    open FSharp.Control.Tasks

    type CloudEvent<'a> =
        { Id: string
          SpecVersion: string
          Source: string
          Type: string
          DataContentType: string
          PubSubName: string option
          TraceId: string option
          Topic: string option
          DataSchema: string option
          Subject: string option
          Time: string option
          Data: 'a
          DataBase64: string option }


    let bindCloudEventAsync<'a> (httpContext: HttpContext) = 
        httpContext.BindJsonAsync<CloudEvent<'a>>()

    let bindCloudEventDataAsync<'a> (httpContext: HttpContext) = 
        task {
            let! event = bindCloudEventAsync<'a> httpContext
            return event.Data
        }