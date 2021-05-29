namespace Domain

[<AutoOpen>]
module Models = 

    type DocStoreProvider =
        | YaCloud

    type DocStore = {
        Url: string
        Provider: DocStoreProvider
    }

    //
    type DocTextExtarctedProvider = 
        |  YaOCR

    type DocExtarctedText = {
        Words: string array
        Provider: DocTextExtarctedProvider
    }

    //
    type DocLabeledProvider = 
        |  CustomPy

    type DocLabel = 
        | Passport
        | ForeignPassport
        | DriverLicense
        | Visa

    type DocLabeled = {
        Label: DocLabel
        Provider: DocLabeledProvider
    }
