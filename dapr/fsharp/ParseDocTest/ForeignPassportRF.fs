module ForeignRFTests

open Expecto
open ParseDoc
open Domain

let Case1Words =
    "российская федерация russian federation подпись владельца holder's signature rus российская федерация/russian fedtion паспорт/ passport тип/ туре код государства /code of выдачи rus issuing state номер паспорта/ passport no. 72 2288239 фамилия/ surname путилов| putilov имя/ given names максим александрович| maxim гражданство/ nationality российская федерация/russian federation дата рождения /date of birth 11.03.1980 пол/sex место рождения /place of birthm/m челябинская обл./ ussr дата выдачи/date of issue учетная запись орган, выдавший документ /authority 06.12.2012 фмс 77001 дата окончания срока/ date of expiry действия подпись владельца/ holder's signature 06.12.2022 p<rusputilov<<maxim<<<<<<<<<<<<<<<<<<<<<<<<<< 7222882395rus8003111m2212061<<<<<<<<"

let Case2Words =
    "12:388 4g all 100% российская федерация russian federation российская федерация t/passport тип туро код государства/ code of№ паспорта/ passport no. выдачи rus issuing state/ surname 65n»2321104 путилов/ putilov given names лев максимович/ lev гражданство/ nationality российская/ russian федерация/ federation дата рождения/ date of birth дичный вход/ porsonal no место рождения/ place of birth d) поп/ sex 07.03.2017 г. mockba/russia м/м/ date of 18.05.2017 орган, документ/ для окончания срока действия/ date of 18.05.2022 1017 rusputilov<<lev<<<<<<<<<<<<<<<<<<<<<<<<< 523211048rus1703076m2205186<<<<<ck скопировать текстс изображения° поделиться изменить объектив удалить"

let Case3Words =
    "подпись владельца holder's signature rus российская федерация/russian federation паспорт, 94ssport tun/typep код государства /code of of выдачи issuing state rus фамилия/surname юсупова yusupova имя/given names наргиза абдукахкаровна/ nargiza гражданство/ nationality российская федерация/russian federation дата рождения /date of birth 14.04.1979 пол/sex место рождения /place of birth ж/f г. ташкент/ ussr дата выдачи/date of issue 06.06.2014 дата окончания срока /date of expiry действия номер паспорта /passport no. 72 8960624 учетная запись орган, выдавший документ /authority фмс 77115 подпись владельца/ holder's signature 06.06.2024 rusyusupova<<nargiza<<<<<<<<<<<<<<<<<<<<< 39606246rus7904141f2406064<<<<<<<<<<<<<<o"

[<Tests>]
let tests =
    let resources = loadResources ()

    testList
        "ForeignPassportRF"
        [ testCase
            "case 1"
            (fun _ ->
                let expected: ForeignPassportRF =
                    { LastName = "путилов"
                      LastNameEn = "putilov"
                      FirstName = "максим"
                      FirstNameEn = "maxim"
                      MiddleName = "александрович"
                      MiddleNameEn = null
                      Identifier = "72 2288239"
                      Issuer = "фмс 77001"
                      IssueDate = "06.12.2012"
                      ExpiryDate = "06.12.2022"
                      Sex = "male"
                      DateOfBirth = "11.03.1980"
                      PlaceOfBirth = "челябинская обл"
                      PlaceOfBirthEn = "ussr"
                      Type = "C" }

                let actual =
                    ForeignPassportRF.parse resources Case1Words

                Expect.equal actual expected "unexpected result")
          testCase
              "case 2"
              (fun _ ->
                  let expected: ForeignPassportRF =
                      { LastName = "путилов"
                        LastNameEn = "putilov"
                        FirstName = "лев"
                        FirstNameEn = "lev"
                        MiddleName = "максимович"
                        MiddleNameEn = null
                        Identifier = "65 2321104"
                        Issuer = null
                        IssueDate = "18.05.2017"
                        ExpiryDate = "18.05.2022"
                        Sex = "male"
                        DateOfBirth = "07.03.2017"
                        PlaceOfBirth = "г. mockba"
                        PlaceOfBirthEn = "russia"
                        Type = "C" }

                  let actual =
                      ForeignPassportRF.parse resources Case2Words

                  Expect.equal actual expected "unexpected result")
          testCase
              "case 3"
              (fun _ ->
                  let expected: ForeignPassportRF =
                      { LastName = "юсупова"
                        LastNameEn = "yusupova"
                        FirstName = "наргиза"
                        FirstNameEn = "nargiza"
                        MiddleName = null
                        MiddleNameEn = null
                        Identifier = "72 8960624"
                        Issuer = "фмс 77115"
                        IssueDate = "06.06.2014"
                        ExpiryDate = "06.06.2024"
                        Sex = "female"
                        DateOfBirth = "14.04.1979"
                        PlaceOfBirth = "г. ташкент"
                        PlaceOfBirthEn = "ussr"
                        Type = "C" }

                  let actual =
                      ForeignPassportRF.parse resources Case3Words

                  Expect.equal actual expected "unexpected result")

          ]
