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
    let tryCreateStateWithMetadataAsync { Dapr = dapr; Logger = logger } storeName id doc metadata =
        task {
            let! res = dapr.TrySaveStateAsync(storeName, id, doc, NEW_ETAG, metadata = metadata)

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

    let tryCreatStateAsync env storeName id doc = tryCreateStateWithMetadataAsync env storeName id doc (readOnlyDict [])    

    // Create new item even if exists
    let createStateWithMetadataAsync { Dapr = dapr; Logger = logger } storeName id doc metadata =
        task {
            do! dapr.SaveStateAsync(storeName, id, doc, metadata = metadata)

            logTrace3 logger "{stateStore} record with {id} updated with new {document}" storeName id doc
        }

    let creatStateTTLAsync env storeName id doc (ttl: int) = createStateWithMetadataAsync env storeName id doc (readOnlyDict [ "ttlInSeconds", ttl.ToString() ])

    let creatStateAsync env storeName id doc = createStateWithMetadataAsync env storeName id doc (readOnlyDict [])    

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

    /// Create new item or fail if already exists
    let getStateAsync<'a> { Dapr = dapr; Logger = logger } storeName id =
        task {
            let! res = dapr.GetStateAsync<'a>(storeName, id)

            match box res with
            | null  ->
                logWarn2 logger "{stateStore} get value for {id} not found" storeName id                
                return None
            | _ ->
                logTrace3 logger "{stateStore} get value for {id} return {res}" storeName id res
                return Some res

        }

    /// Create new item or fail if already exists
    let deleteStateAsync { Dapr = dapr; Logger = logger } storeName id =
        dapr.DeleteStateAsync(storeName, id)
