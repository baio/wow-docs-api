module PassportRFTests

open Expecto
open ParseDoc

let Case1Words =
    "российская федерация отделением уфмс россии по челябинской областив гор. трехгорный сл 21.10.2013 123-456 путилов максим сл александрович мух. 11.03.1980 гор. златоуст челябинской обл. pnrusputilov<<maksim<aleksandrovi3<<<<<<<<<< 7824678082rus8003111m<<<<<<<3131223740049<50"

[<Tests>]
let tests =
    let resources = loadResources ()

    testList
        "PassportRF"
        [ testCase
              "case 1"
              (fun _ ->
                  let expected: PassportRF.Data =
                      { Issuer =
                            "российская федерация отделением уфмс россии по челябинской областив гор. трехгорный сл"
                        IssuerCode = "123-456"
                        BirthDate = "11.03.1980"
                        Number = "782467808"
                        FirstName = "максим"
                        LastName = "путилов"
                        MiddleName = "александрович"
                        Sex = "male"
                        IssueDate = "21.10.2013"
                        BirthPlace = "гор. златоуст челябинской обл." }

                  let actual =
                      PassportRF.parse resources.RuNames Case1Words

                  printfn "%O" actual
                  Expect.equal actual expected "unexpected result") ]
