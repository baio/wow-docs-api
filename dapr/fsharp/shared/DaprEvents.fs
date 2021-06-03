namespace Shared

[<AutoOpen>]
module DaprEvents =

    open Domain.Events
    open FSharp.Dapr

    let publishDocRead x =
        x
        |> publishEventAsync<DocReadEvent> DAPR_DOC_PUB_SUB DAPR_TOPIC_DOC_READ

    let publishDocStored x =
        x
        |> publishEventAsync<DocStoredEvent> DAPR_DOC_PUB_SUB DAPR_TOPIC_DOC_STORED

    let subscribeDocRead x =
        x
        |> subscribeDapr<DaprAppEnv, DocReadEvent, bool> DAPR_DOC_PUB_SUB DAPR_TOPIC_DOC_READ