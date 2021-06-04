namespace FSharp.Dapr

[<AutoOpen>]
module State =

    open Dapr.Client
    open FSharp.Control.Tasks

    type UpdateResult<'a> =
        { IsSuccess: bool
          ETag: string
          Id: string
          Doc: 'a }

    let private NEW_ETAG = "-1"

    /// Create new item or fail if already exists
    let tryCreateStateAsync { Dapr = dapr; Logger = logger } storeName id doc =
        task {
            let! res = dapr.TrySaveStateAsync(storeName, id, doc, NEW_ETAG)

            match res with
            | true ->
#if TRACE                
                logTrace2 logger "{stateStore} updated with new {document}" storeName doc
#endif                
            | false ->
                logWarn2 logger "{stateStore} failed to update, {docKey} already exists" storeName id
#if TRACE                
                logTrace2 logger "{stateStore} failed to update, {document} already exists" storeName doc
#endif                

            return res
        }

    /// Find item and update it if exists
    /// If item is not exists then create new and then update it
    let tryUpdateOrCreateStateAsync<'a> { Dapr = dapr; Logger = logger } storeName id createFun (updateFun: 'a -> 'a) =
        task {
            let! docEntry = dapr.GetStateEntryAsync<'a>(storeName, id)

            let (etag, doc) =
                match box docEntry.Value with
                | null ->
                    // document still not created
                    (NEW_ETAG, createFun id)
                | _ -> (docEntry.ETag, docEntry.Value)

            let doc = updateFun doc

            let! res = dapr.TrySaveStateAsync(storeName, id, doc, etag)

            let res =
                { IsSuccess = res
                  ETag = etag
                  Id = id
                  Doc = doc }

            match res.IsSuccess with
            | true ->
#if TRACE
                logTrace3 logger "{stateStore} document with {docKey} is updated with {result}" storeName id res
#endif                
            | false ->
                logWarn3 logger "{stateStore} document with {docKey} fail to update with {etag}" storeName id etag
#if TRACE                
                logTrace3 logger "{stateStore} document with {docKey} fail to update with {result}" storeName id res
#endif                
        }
