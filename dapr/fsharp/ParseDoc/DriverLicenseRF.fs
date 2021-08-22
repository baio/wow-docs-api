[<RequireQualifiedAccess>]
module ParseDoc.DriverLicenseRF

open Domain
open System.Text.RegularExpressions

let parseIssuer str =
    let issuer = Regex "гибдд\s\d+"

    let issuerMatch = issuer.Match str
    iim issuerMatch

let parseIssuerEn str =
    let issuer = Regex "gibdd\s\d+"

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
    let number = Regex "\d{2,3}\s\d{2}\s\d{6}"
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


let parseIssuerRegion str =
    let birthPlace =
        Regex "8\.+\s([а-я\s\.]+)([a-z\s]+).*6\."

    let birthPlaceMatch = birthPlace.Match str
    iimg birthPlaceMatch 1

let parseIssuerRegionEn str =
    let birthPlace = Regex "8\.+\s[а-я\s\.]+([a-z\s]+).*6\."

    let birthPlaceMatch = birthPlace.Match str
    iimg birthPlaceMatch 1


let parseRegionOfBirth (str: string) =
    let regex = Regex "\d+\s([а-я\s]+).*4a"

    let m = regex.Match str
    iimg m 1

let parseRegionOfBirthEn (str: string) =
    let regex = Regex "\d+\s[а-я\s\.]+([a-z\s]+).*4a"

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
      Issuer = parseIssuer words
      IssuerEn = parseIssuerEn words
      IssueDate = parseIssueDate words
      ExpiryDate = parseExpiryDate words
      DateOfBirth = parseBirthDate words
      RegionOfBirth = parseRegionOfBirth words
      RegionOfBirthEn = parseRegionOfBirthEn words
      IssuerRegion = parseIssuerRegion words
      IssuerRegionEn = parseIssuerRegionEn words
      Categories = null }: DriverLicenseRF
