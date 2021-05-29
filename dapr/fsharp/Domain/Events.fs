namespace Domain

[<AutoOpen>]
module Events = 

    type DocStoreProvider =
        | YaCloud

    type DocStore = {
        Url: string
        Provider: DocStoreProvider
    }

    type DocStored = { DocKey: string; DocStore: DocStore; }

    type DocRead = { DocKey: string; DocContent: string; }


