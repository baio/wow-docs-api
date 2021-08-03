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

    let publishDocTextExtracted x =
        x
        |> publishEventAsync<DocTextExtractedEvent> DAPR_DOC_PUB_SUB DAPR_TOPIC_DOC_TEXT_EXTRACTED

    let publishDocLabeled x =
        x
        |> publishEventAsync<DocLabeledEvent> DAPR_DOC_PUB_SUB DAPR_TOPIC_DOC_LABELED

    let publishDocParsed x =
        x
        |> publishEventAsync<DocParsedEvent> DAPR_DOC_PUB_SUB DAPR_TOPIC_DOC_PARSED

    let subscribeDocRead x =
        x
        |> subscribeDapr<DaprAppEnv, DocReadEvent, bool> DAPR_DOC_PUB_SUB DAPR_TOPIC_DOC_READ

    let subscribeDocStored x =
        x
        |> subscribeDapr<DaprAppEnv, DocStoredEvent, bool> DAPR_DOC_PUB_SUB DAPR_TOPIC_DOC_STORED

    let subscribeDocTextExtracted x =
        x
        |> subscribeDapr<DaprAppEnv, DocTextExtractedEvent, bool> DAPR_DOC_PUB_SUB DAPR_TOPIC_DOC_TEXT_EXTRACTED

    let subscribeDocLabeled x =
        x
        |> subscribeDapr<DaprAppEnv, DocLabeledEvent, bool> DAPR_DOC_PUB_SUB DAPR_TOPIC_DOC_LABELED

    let subscribeDocParsed x =
        x
        |> subscribeDapr<DaprAppEnv, DocParsedEvent, unit> DAPR_DOC_PUB_SUB DAPR_TOPIC_DOC_PARSED
