namespace Shared 

[<AutoOpen>]
module DaprState = 
    
    open Dapr.Client
    open FSharp.Control.Tasks

    type UpdateResult<'a> = {
        IsSuccess: bool
        ETag: string
        Id: string
        Doc: 'a
    }

    let private NEW_ETAG = "-1"

    /// Create new item or fail if already exists
    let tryCreateStateAsync (dapr: DaprClient) storeName id doc =
        dapr.TrySaveStateAsync(storeName, id, doc, NEW_ETAG)
    
    /// Find item and update it if exists
    /// If item is not exists then create new and then update it
    let tryUpdateOrCreateStateAsync<'a> (dapr: DaprClient) storeName id createFun (updateFun: 'a -> 'a) =
        task {
            let! docEntry = dapr.GetStateEntryAsync<'a>("statestore", id)
            let (etag, doc) =
                match box docEntry.Value with
                | null -> 
                    // document still not created
                    (NEW_ETAG, createFun id)
                | _  -> (docEntry.ETag, docEntry.Value)
            
            let doc = updateFun doc

            let! res = dapr.TrySaveStateAsync(storeName, id, doc, etag)
            
            return {
                IsSuccess = res
                ETag = etag
                Id = id
                Doc = doc
            }
        }
