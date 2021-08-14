namespace Domain

[<AutoOpen>]
module Models =

    type DocStoreProvider = | YaCloud

    type DocStore =
        { Url: string
          Provider: DocStoreProvider }

    //
    type DocTextExtarctedProvider = | YaOCR

    [<RequireQualifiedAccessAttribute>]
    type DocExtractedResult =
        | Words of string
        | Error of DomainError
    type DocExtarctedText =
        { Result: DocExtractedResult
          Provider: DocTextExtarctedProvider }

    //
    type DocLabeledProvider = | CustomPy

    type DocLabel =
        | Passport
        | ForeignPassport
        | DriverLicense
        | Visa

    type DocLabeled =
        { Label: DocLabel
          Provider: DocLabeledProvider }

    type DocParsed = { ParsedDoc: ParsedDoc }
