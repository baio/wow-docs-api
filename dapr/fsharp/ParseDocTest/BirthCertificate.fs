module BirthCerticiateTests

open Expecto
open ParseDoc
open Domain

let Case1Words =
    "свидетельствоо рождении турсунов назар дмитриевич имя, отчество родился(лась) 12/01/2011 число, месяц, год (цифрамии прописью) двенадцатого января две тысячи одиннадцатого года место рождения г. новосибирск, новосибирская область, россияо чем 2011 года февраля месяца 01 числа составлена запись актао рождении№ отец турсунов дмитрий александрович имй, гражданин россии национальное отца) мать тұрсунова фамилия евгения сергеевна гражданка россии русская национальность (вносится по желанию матери) место государственной регистрации отдел загс дзержинского района г. новосибирска наименование органа записи актов гражданского состояния управления по делам загс новосибирской области город дата выдачи« 26 апреля 2011 г. уководитель органа актов гражданского состояния h. в. ильющенко ii-et 619418 гознак, мпф, москва, 1998."

[<Tests>]
let tests =
    let resources = loadResources ()

    testList
        "BirthCretificateRF"
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
                      |> Some

                  let actual =
                      DriverLicenseRF.parse resources Case1Words

                  Expect.equal actual expected "unexpected result") ]
