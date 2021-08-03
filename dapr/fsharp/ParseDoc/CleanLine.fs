[<AutoOpen>]
module ParseDoc.CleanLine

let cleanLine words =
    words
    |> Seq.filter (fun f -> (len f) > 1)
    |> String.concat " "
    |> toLower
