namespace QueryStore.Formatted

[<RequireQualifiedAccess>]
module ForeignPassportRF =

    open Domain
    open Utils


    type ForeignPassportRfDTO =
        { Kind: string
          LastName: string
          LastNameEn: string
          FirstName: string
          FirstNameEn: string
          MiddleName: string
          MiddleNameEn: string
          Identifier: string
          Issuer: string
          IssueDate: string
          ExpiryDate: string
          Sex: string
          DateOfBirth: string
          PlaceOfBirth: string
          PlaceOfBirthEn: string
          Type: string }

    let format (foreignPassportRF: ForeignPassportRF) : ForeignPassportRfDTO =
        { Kind = "passport-foreign-rf"
          LastName = foreignPassportRF.LastName
          LastNameEn = foreignPassportRF.LastNameEn
          FirstName = foreignPassportRF.FirstName
          FirstNameEn = foreignPassportRF.FirstNameEn
          MiddleName = foreignPassportRF.MiddleName
          MiddleNameEn = foreignPassportRF.MiddleNameEn
          Identifier = foreignPassportRF.Identifier
          Issuer = foreignPassportRF.Issuer
          IssueDate = formatDateToISO foreignPassportRF.IssueDate
          ExpiryDate = formatDateToISO foreignPassportRF.ExpiryDate
          Sex = foreignPassportRF.Sex
          DateOfBirth = formatDateToISO foreignPassportRF.DateOfBirth
          PlaceOfBirth = foreignPassportRF.PlaceOfBirth
          PlaceOfBirthEn = foreignPassportRF.PlaceOfBirthEn
          Type = foreignPassportRF.Type }
