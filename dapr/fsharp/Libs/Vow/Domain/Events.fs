namespace Domain

[<AutoOpen>]
module Events =

    type DocReadEvent = { DocKey: string; DocContent: string }

    type DocStoredEvent = { DocKey: string; DocStore: DocStore }

    type DocTextExtractedEvent =
        { DocKey: string
          DocExtractedText: DocExtarctedText }

    type DocLabeledEvent =
        { DocKey: string
          DocLabeled: DocLabeled }

    type DocParsedEvent =
        { DocKey: string
          DocExtractedText: DocExtarctedText
          DocParsed: DocParsed }
