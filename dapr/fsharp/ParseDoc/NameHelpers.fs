[<AutoOpen>]
module internal ParseDoc.NameHelpers

let isSomeName c str = c |> Seq.contains str

let findSomeName c =
    Seq.tryFind (isSomeName c) >> toNullable

type ParsedName =
    { FirstName: string
      LastName: string
      MiddleName: string }

let parseNames (resources: Resources.Names) (line: string) =
    let words = line.Split " "
    { FirstName = findSomeName words resources.FirstNames
      LastName = findSomeName words resources.LastNames
      MiddleName = findSomeName words resources.MiddleNames }
