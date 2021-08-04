namespace FSharp.Dapr

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
          PubSubName: string
          TraceId: string
          Topic: string
          DataSchema: string
          Subject: string
          Time: string
          Data: 'a
          DataBase64: string }

    let bindCloudEventAsync<'a> (httpContext: HttpContext) =
        httpContext.BindJsonAsync<CloudEvent<'a>>()

    let bindCloudEventDataAsync<'a> (httpContext: HttpContext) =
        task {
            let! event = bindCloudEventAsync<'a> httpContext
            return event.Data
        }
