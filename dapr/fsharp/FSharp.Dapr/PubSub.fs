namespace FSharp.Dapr

open System.IO
open System.Text
open Microsoft.AspNetCore.Http

[<AutoOpen>]
module PubSub =
    open Giraffe
    open FSharp.Control.Tasks
    open System.Threading.Tasks

    type SubscribeHttpHandler<'x> = 'x -> HttpHandler

    let private REQUEST_BUFFER_SIZE = 99999999

    let private readStream stream =
        let reader =
            new StreamReader(
                stream,
                encoding = Encoding.UTF8,
                detectEncodingFromByteOrderMarks = false,
                bufferSize = REQUEST_BUFFER_SIZE,
                leaveOpen = true
            )

        task {
            let! res = reader.ReadToEndAsync()
            stream.Position <- int64 0
            return res
        }

    let private readRequestBody (request: HttpRequest) =
        request.EnableBuffering(REQUEST_BUFFER_SIZE)
        readStream request.Body

    let publishEventAsync<'a> pubSubName topicName { Dapr = dapr; Logger = logger } (event: 'a) =
#if TRACE
        logTrace3 logger "Publishing {pubSubName} {topicName} {event}" pubSubName topicName event
#endif
        dapr.PublishEventAsync(pubSubName, topicName, event)

    let subscribeDapr'<'x, 'e> (pubSubName: string) (topicName: string) (handler: 'e -> SubscribeHttpHandler<'x>) =
        pubSubName,
        topicName,
        fun env next (ctx: HttpContext) ->
            task {
#if TRACE
                let! bodyStr = readRequestBody ctx.Request

                logTrace3
                    (ctx.GetLogger())
                    "Subscription {pubSubName} {topicName} called with {body}"
                    pubSubName
                    topicName
                    bodyStr
#endif
                let! event = bindCloudEventDataAsync<'e> ctx
#if TRACE
                logTrace3
                    (ctx.GetLogger())
                    "Subscription {pubSubName} {topicName} called with parsed {event}"
                    pubSubName
                    topicName
                    event
#endif
                return! handler event env next ctx
            }

    let subscribeDapr<'x, 'e, 'r> pubSubName topicName (handler: 'e -> 'x -> Task<'r>) =
        subscribeDapr'<'x, 'e>
            pubSubName
            topicName
            (fun event env next ctx ->
                task {
                    let! resultEvent = handler event env
                    return! json resultEvent next ctx
                })
