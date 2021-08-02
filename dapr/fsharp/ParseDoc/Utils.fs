[<AutoOpen>]
module internal ParseDoc.Utils

let toNullable =
    (function
    | Some x -> x
    | None -> null)
