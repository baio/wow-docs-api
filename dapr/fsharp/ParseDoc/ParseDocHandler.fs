[<AutoOpen>]
module ParseDoc.ParseDocHandler

open Domain
open FSharp.Control.Tasks

let private isForeignPassportRF (foreignPassportRF: ForeignPassportRF) =
    foreignPassportRF.FirstNameEn |> isNull |> not
    || foreignPassportRF.LastNameEn |> isNull |> not

let private isPassportRF (passportRF: PassportRF) = passportRF.BirthDate |> isNull |> not

let private parseForeignPassportRF resources words =
    let doc = ForeignPassportRF.parse resources words

    if isForeignPassportRF doc then
        Some(ForeignPassportRF doc)
    else
        None

let private parsePassportRF resources words =
    let doc = PassportRF.parse resources.RuNames words

    if isPassportRF doc then
        Some(PassportRF doc)
    else
        None

let parsers resources =
    [ DriverLicenseRF.parse resources
      >> Option.map (DriverLicenseRF)
      DriverLicenseOldRF.parse resources
      >> Option.map (DriverLicenseRF)
      parseForeignPassportRF resources
      parsePassportRF resources ]

let parseDoc (resources: Resources) words _ =
    parsers resources
    |> Seq.tryPick (fun p -> p words)
