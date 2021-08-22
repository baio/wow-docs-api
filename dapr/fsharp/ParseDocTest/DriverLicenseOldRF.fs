module DriverLicenseOldRFTests

open Expecto
open ParseDoc
open Domain

let Case1Words =
    "permis de conduire фамилия путилов putilov имя максим maksim отчество александрович датаи место рождения 11.03.1980 челябинская область- chelyabin место жительства челябинская область- chelyabin подпись владельца выдано mbd- москва выдачи 26.06.2009 до 26.06.2019 a. 77 080296"

[<Tests>]
let tests =
    let resources = loadResources ()

    testList
        "DriverLicenseOldRF"
        [ testCase
              "case 1"
              (fun _ ->

                  let expected =
                      { LastName = "путилов"
                        LastNameEn = "putilov"
                        FirstName = "максим"
                        FirstNameEn = "maksim"
                        MiddleName = "александрович"
                        MiddleNameEn = null
                        Identifier = "77 080296"
                        Issuer = null
                        IssuerEn = null
                        IssueDate = "26.06.2009"
                        ExpiryDate = "26.06.2019"
                        DateOfBirth = "11.03.1980"
                        RegionOfBirth = "челябинская область"
                        RegionOfBirthEn = null
                        IssuerRegion = "москва"
                        IssuerRegionEn = null
                        Categories = null }

                  let actual =
                      DriverLicenseOldRF.parse resources Case1Words


                  Expect.equal actual expected "unexpected result") ]
