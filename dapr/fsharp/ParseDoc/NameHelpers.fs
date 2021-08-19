[<AutoOpen>]
module internal ParseDoc.NameHelpers

open System.Text.RegularExpressions

let isSomeName c str = c |> Seq.contains str

let findSomeName c =
    Seq.tryFind (isSomeName c) >> toNullable

type ParsedName =
    { FirstName: string
      LastName: string
      MiddleName: string }

let private cleanWord (str: string) =
    match str with
    | null -> null
    | s -> Regex.Replace(s, "\W+", "")

let parseNames (resources: Resources.Names) (line: string) =
    let words = line.Split " " |> Seq.map cleanWord

    { FirstName = findSomeName words resources.FirstNames
      LastName = findSomeName words resources.LastNames
      MiddleName = findSomeName words resources.MiddleNames }
