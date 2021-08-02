[<AutoOpen>]
module internal ParseDoc.RegexHelpers

open System
open System.Text.RegularExpressions


let iim (m: Match) = if m.Success then m.Value else null

let iimg (m: Match) i =
    if (m.Groups.Count > i) then
        m.Groups.[i].Value
    else
        null

let iimc (m: MatchCollection) i : string =
    if (m.Count > i) then
        m.[i].Value
    else
        null

let iimcg (m: MatchCollection) i j =
    if (i > 0
        && m.Count > i
        && m.[i].Groups.Count > j
        && m.[i].Groups.[j].Captures.Count > 0) then
        m.[i].Groups.[j].Captures.[0].Value
    else
        null

let iimlg (m: MatchCollection) j : string = iimcg m (m.Count - 1) j

let toNullable =
    (function
    | Some x -> x
    | None -> null)
