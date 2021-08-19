module ForeignRFTests

open Expecto
open ParseDoc
open Domain

let Case1Words =
    "российская федерация russian federation подпись владельца holder's signature rus российская федерация/russian fedtion паспорт/ passport тип/ туре код государства /code of выдачи rus issuing state номер паспорта/ passport no. 72 2288239 фамилия/ surname путилов| putilov имя/ given names максим александрович| maxim гражданство/ nationality российская федерация/russian federation дата рождения /date of birth 11.03.1980 пол/sex место рождения /place of birthm/m челябинская обл./ ussr дата выдачи/date of issue учетная запись орган, выдавший документ /authority 06.12.2012 фмс 77001 дата окончания срока/ date of expiry действия подпись владельца/ holder's signature 06.12.2022 p<rusputilov<<maxim<<<<<<<<<<<<<<<<<<<<<<<<<< 7222882395rus8003111m2212061<<<<<<<<"


[<Tests>]
let tests =
    let resources = loadResources ()

    ftestList
        "ForeignPassportRF"
        [ testCase
              "case 1"
              (fun _ ->
                  let expected: ForeignPassportRF =
                      { LastName = ""
                        LastNameEn = ""
                        FirstName = ""
                        FirstNameEn = ""
                        MiddleName = ""
                        MiddleNameEn = ""
                        Identifier = ""
                        Issuer = ""
                        IssueDate = ""
                        ExpiryDate = ""
                        Sex = ""
                        DateOfBirth = ""
                        PlaceOfBirth = ""
                        PlaceOfBirthEn = ""
                        Type = "C" }

                  let actual =
                      ForeignPassportRF.parse resources.RuNames Case1Words

                  printfn "%O" actual
                  Expect.equal actual expected "unexpected result")

         ]
