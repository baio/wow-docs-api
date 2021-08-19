[<AutoOpen>]
module ParseDoc.ParseDocHandler

open Domain
open FSharp.Control.Tasks

let private isForeignPassportRF (foreignPassportRF: ForeignPassportRF) =
    foreignPassportRF.FirstNameEn |> isNull |> not
    || foreignPassportRF.LastNameEn |> isNull |> not

let private isPassportRF (passportRF: PassportRF) = passportRF.BirthDate |> isNull |> not

let parseDoc (resources: Resources) words _ =

    let foreignPassportRF = ForeignPassportRF.parse resources words

    if isForeignPassportRF foreignPassportRF then
        Some(ForeignPassportRF foreignPassportRF)
    else
        let passportRF = PassportRF.parse resources.RuNames words

        if isPassportRF passportRF then
            Some(PassportRF passportRF)
        else
            None
