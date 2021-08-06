namespace Shared

[<AutoOpen>]
module DaprStore =
    open Domain
    open FSharp.Dapr

    type TestData = {
        Message: string
    }

    let TTL_FOR_STORE_PARSED_DOC = 60    

    let addDocParsed env id (docParsed: DocParsed) =
        creatStateTTLAsync env DAPR_DOC_STATE_STORE id docParsed TTL_FOR_STORE_PARSED_DOC

    let getDocParsed env id =
        getStateAsync<DocParsed> env DAPR_DOC_STATE_STORE id

    let addTest env id (data: TestData) =
        creatStateAsync env DAPR_DOC_STATE_STORE id data

    let getTest env id =
        getStateAsync<TestData> env DAPR_DOC_STATE_STORE id
