namespace Domain

[<AutoOpen>]
module ParsedDocModels =
    type PassportRF =
        { Issuer: string
          IssuerCode: string
          BirthDate: string
          Identifier: string
          FirstName: string
          LastName: string
          MiddleName: string
          Sex: string
          IssueDate: string
          BirthPlace: string }

    type ForeignPassportRF =
        { LastName: string
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


    type DriverLicenseRF =
        { LastName: string
          LastNameEn: string
          FirstName: string
          FirstNameEn: string
          MiddleName: string
          MiddleNameEn: string
          Identifier: string
          Issuer: string
          IssuerEn: string
          IssueDate: string
          ExpiryDate: string
          DateOfBirth: string
          RegionOfBirth: string
          RegionOfBirthEn: string
          IssuerRegion: string
          IssuerRegionEn: string
          Categories: string }


    type ParsedDoc =
        | ErrorDoc of DomainError
        | PassportRF of PassportRF
        | ForeignPassportRF of ForeignPassportRF
        | DriverLicenseRF of DriverLicenseRF
