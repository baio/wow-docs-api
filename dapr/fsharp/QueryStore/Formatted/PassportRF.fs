namespace QueryStore.Formatted

[<RequireQualifiedAccess>]
module PassportRF =

    open Domain
    open Utils

    type PassportRfDTO =
        { Kind: string
          LastName: string
          FirstName: string
          MiddleName: string
          Identifier: string
          Issuer: string
          IssueDate: string
          Sex: string
          DateOfBirth: string
          PlaceOfBirth: string
          DepartmentCode: string }


    let format (passportRF: PassportRF) : PassportRfDTO =
        { Kind = "passport-rf"
          LastName = passportRF.LastName
          FirstName = passportRF.FirstName
          MiddleName = passportRF.MiddleName
          Identifier = passportRF.Identifier
          Issuer = passportRF.Issuer
          IssueDate = formatDateToISO passportRF.IssueDate
          Sex = passportRF.Sex
          DateOfBirth = formatDateToISO passportRF.BirthDate
          PlaceOfBirth = passportRF.BirthPlace
          DepartmentCode = passportRF.IssuerCode }
