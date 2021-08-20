[<RequireQualifiedAccess>]
module ParseDoc.ForeignPassportRF

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
    let number = Regex "\d{2}\D{0,2}\d{7}"
    let numberMatch = number.Match str
    let res = iim numberMatch

    if not (isNull res) then
        Regex("\D+").Replace(res, " ")
    else
        null

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
        Regex "place of birth.*?([а-я.]+\s[a-я]{0,})\/?"

    let birthPlaceMatch = birthPlace.Match str
    iimg birthPlaceMatch 1

let parseBirthPlaceEn (str: string) =
    if str.Contains "ussr" then
        "ussr"
    else
        "russia"

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
      IssueDate = parseIssueDate words
      ExpiryDate = parseExpiryDate words
      Sex = parseSex words
      DateOfBirth = parseBirthDate words
      PlaceOfBirth = parseBirthPlace words
      PlaceOfBirthEn = parseBirthPlaceEn words
      Type = "C" }
