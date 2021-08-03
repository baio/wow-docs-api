[<AutoOpen>]
module internal ParseDoc.Utils

let toNullable =
    (function
    | Some x -> x
    | None -> null)

let len (str: string) = str.Length

let toLower (str: string) = str.ToLower()