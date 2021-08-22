module DriverLicenseRFTests

open Expecto
open ParseDoc
open Domain

let Case1Words =
    "rus водительское удостоверение permis de conduire driving licence 1. путилов putilov 2. максим александрович maksim aleksandrovich 3. 11.03.1980 челябинская обл. cheliabinskaia oblast 4a) 24.04.2019 4с) гибдд 5015 gibdd 5015 4b) 24.04.2029 5. 99 08 677490 8. челябинская обл. cheliabinskaia obl. 6. 7."

[<Tests>]
let tests =
    let resources = loadResources ()

    testList
        "DriverLicenseRF"
        [ testCase
              "case 1"
              (fun _ ->

                  let expected =
                      { LastName = "путилов"
                        LastNameEn = "putilov"
                        FirstName = "максим"
                        FirstNameEn = "maksim"
                        MiddleName = "александрович"
                        MiddleNameEn = "aleksandrovich"
                        Identifier = "99 08 677490"
                        Issuer = "гибдд 5015"
                        IssuerEn = "gibdd 5015"
                        IssueDate = "24.04.2019"
                        ExpiryDate = "24.04.2029"
                        DateOfBirth = "11.03.1980"
                        RegionOfBirth = "челябинская обл"
                        RegionOfBirthEn = "cheliabinskaia oblast"
                        IssuerRegion = "челябинская обл."
                        IssuerRegionEn = "cheliabinskaia obl"
                        Categories = null }

                  let actual =
                      DriverLicenseRF.parse resources Case1Words


                  Expect.equal actual expected "unexpected result") ]
