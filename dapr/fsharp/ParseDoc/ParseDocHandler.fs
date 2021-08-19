[<AutoOpen>]
module ParseDoc.ParseDocHandler

open Domain
open FSharp.Control.Tasks

let private isPassportRF (passportRF: PassportRF) = passportRF.BirthDate |> isNull |> not

let parseDoc (resources: Resources) words _ =

    let passportRF = PassportRF.parse resources.RuNames words

    if isPassportRF passportRF then
        Some(PassportRF passportRF)
    else
        None