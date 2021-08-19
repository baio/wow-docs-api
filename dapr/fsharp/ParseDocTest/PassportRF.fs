module PassportRFTests

open Expecto
open ParseDoc
open Domain

let Case1Words =
    "российская федерация отделением уфмс россии по челябинской областив гор. трехгорный сл 21.10.2013 123-456 путилов максим сл александрович мух. 11.03.1980 гор. златоуст челябинской обл. pnrusputilov<<maksim<aleksandrovi3<<<<<<<<<< 7824678082rus8003111m<<<<<<<3131223740049<50"

let Case2Words =
    "01.03.2004 города трехгорный1рация отделом внутренних дел челябинской области 742-050 код фамилия путилов ижи отчество александр николаевич1 муж дата рождения 23.11.1955 место город златоуст челябинской области"

"российская федерация russian federation подпись владельца holder's signature rus российская федерация/russian fedtion паспорт/ passport тип/ туре код государства /code of выдачи rus issuing state номер паспорта/ passport no. 72 2288239 фамилия/ surname путилов| putilov имя/ given names максим александрович| maxim гражданство/ nationality российская федерация/russian federation дата рождения /date of birth 11.03.1980 пол/sex место рождения /place of birthm/m челябинская обл./ ussr дата выдачи/date of issue учетная запись орган, выдавший документ /authority 06.12.2012 фмс 77001 дата окончания срока/ date of expiry действия подпись владельца/ holder's signature 06.12.2022 p<rusputilov<<maxim<<<<<<<<<<<<<<<<<<<<<<<<<< 7222882395rus8003111m2212061<<<<<<<<"

[<Tests>]
let tests =
    let resources = loadResources ()

    testList
        "PassportRF"
        [ testCase
            "case 1"
            (fun _ ->
                let expected: PassportRF =
                    { Issuer = "российская федерация отделением уфмс россии по челябинской областив гор. трехгорный сл"
                      IssuerCode = "123-456"
                      BirthDate = "11.03.1980"
                      Identifier = "782467808"
                      FirstName = "максим"
                      LastName = "путилов"
                      MiddleName = "александрович"
                      Sex = "male"
                      IssueDate = "21.10.2013"
                      BirthPlace = "гор. златоуст челябинской обл." }

                let actual =
                    PassportRF.parse resources.RuNames Case1Words

                printfn "%O" actual
                Expect.equal actual expected "unexpected result")

          testCase
              "case 2"
              (fun _ ->
                  let expected: PassportRF =
                      { Issuer = null
                        IssuerCode = "742-050"
                        BirthDate = "23.11.1955"
                        Identifier = null
                        FirstName = "александр"
                        LastName = "путилов"
                        MiddleName = "николаевич"
                        Sex = "male"
                        IssueDate = "01.03.2004"
                        BirthPlace = "место город златоуст челябинской области" }

                  let actual =
                      PassportRF.parse resources.RuNames Case2Words

                  printfn "%O" actual
                  Expect.equal actual expected "unexpected result") ]
