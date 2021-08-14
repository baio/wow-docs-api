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

    type ParsedDoc =
        | ErrorDoc of DomainError
        | PassportRF of PassportRF
