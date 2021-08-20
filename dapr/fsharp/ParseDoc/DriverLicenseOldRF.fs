[<RequireQualifiedAccess>]
module ParseDoc.DriverLicenseOldRF

open Domain
open System.Text.RegularExpressions

let parseIssuer str =
    let issuer = Regex "фмс\s\d+\s"

    let issuerMatch = issuer.Match str
    iim issuerMatch

let parseIssuerCode str =
    let issuerCode = Regex "\d{3}\-\d{3}"
    let issuerCodeMatch = issuerCode.Match str
    iim issuerCodeMatch

let parseBirthDate str =
    let date = Regex "\d{2}\.\d{2}\.\d{4}"
    let dateMatch = date.Matches str
    iimc dateMatch 0

let parseNumber str =
    let number = Regex "\d{2}\s([a-zа-я]+\s)?\d{6}"
    let numberMatch = number.Match str
    iim numberMatch

let parseSex str =
    let sex = Regex "[m|м]\/[m|м]"
    let sexMatch = sex.Match str

    if sexMatch.Success then
        "male"
    else
        "female"


let parseIssueDate str =
    let date = Regex "\d{2}\.\d{2}\.\d{4}"
    let dateMatch = date.Matches str
    iimc dateMatch 1

let parseExpiryDate str =
    let date = Regex "\d{2}\.\d{2}\.\d{4}"
    let dateMatch = date.Matches str
    iimc dateMatch 2


let parseBirthPlace str =
    let birthPlace =
        Regex "место рождения[^а-я]+([а-я\s]+)"

    let birthPlaceMatch = birthPlace.Match str
    iimg birthPlaceMatch 1

let parseBirthPlaceEn (str: string) =
    if str.Contains "ussr" then
        "ussr"
    else
        "russia"

let parseIssuerRegion (str: string) =
    let regex = Regex "выдано[^а-я]+([а-я]+)"

    let m = regex.Match str
    iimg m 1


let parse (resources: Resources) words =
    let names = parseNames resources.RuNames words
    let namesEn = parseNames resources.EnNames words

    { LastName = names.LastName
      LastNameEn = namesEn.LastName
      FirstName = names.FirstName
      FirstNameEn = namesEn.FirstName
      MiddleName = names.MiddleName
      MiddleNameEn = namesEn.MiddleName
      Identifier = parseNumber words
      Issuer = null
      IssuerEn = null
      IssueDate = parseIssueDate words
      ExpiryDate = parseExpiryDate words
      DateOfBirth = parseBirthDate words
      RegionOfBirth = parseBirthPlace words
      RegionOfBirthEn = null
      IssuerRegion = parseIssuerRegion words
      IssuerRegionEn = null
      Categories = null }: DriverLicenseRF
