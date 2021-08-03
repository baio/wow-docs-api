[<RequireQualifiedAccess>]
module ParseDoc.PassportRF

open System.Text.RegularExpressions

type Data =
    { Issuer: string
      IssuerCode: string
      BirthDate: string
      Number: string
      FirstName: string
      LastName: string
      MiddleName: string
      Sex: string
      IssueDate: string
      BirthPlace: string }

let parseIssuer str =
    let issuer =
        Regex "(^|федерация)(.*?)(дата|\d{2}\.\d{2}\.\d{4})"

    let issuerMatch = issuer.Match str
    iimg issuerMatch 2

let parseIssuerCode str =
    let issuerCode = Regex "\d{3}\-\d{3}"
    let issuerCodeMatch = issuerCode.Match str
    iim issuerCodeMatch

let parseBirthDate str =
    let date = Regex "\d{2}\.\d{2}\.\d{4}"
    let dateMatch = date.Matches str
    iimc dateMatch 1

let parseNumber str =
    let number = Regex "\d{9}"
    let numberMatch = number.Match str
    iim numberMatch

let parseSex str =
    let sex = Regex "му[х|ж]\."
    let sexMatch = sex.Match str

    if sexMatch.Success then
        "male"
    else
        let sex = Regex "[х|ж]ен\."
        let sexMatch = sex.Match str

        if sexMatch.Success then
            "female"
        else
            null


let parseIssueDate str =
    let date = Regex "\d{2}\.\d{2}\.\d{4}"
    let dateMatch = date.Matches str
    iimc dateMatch 0

let parseBirthPlace str =
    let birthPlace = Regex "(рожден.*?\s+)(.*?)([a-z]+)"
    let birthPlaceMatch = birthPlace.Matches str
    let res = iimlg birthPlaceMatch 2

    if isNull res then
        let birthPlace =
            Regex "\d{2}\.\d{2}\.\d{4}.*\d{2}\.\d{2}\.\d{4}(.*?)([a-z]+)"

        let birthPlaceMatch = birthPlace.Match str
        iimg birthPlaceMatch 1
    else
        res

let parse resources words =
    let names = parseNames resources words

    { Issuer = parseIssuer words
      IssuerCode = parseIssuerCode words
      BirthDate = parseBirthDate words
      Number = parseNumber words
      FirstName = names.FirstName
      LastName = names.LastName
      MiddleName = names.MiddleName
      Sex = parseSex words
      IssueDate = parseIssueDate words
      BirthPlace = parseBirthPlace words }
