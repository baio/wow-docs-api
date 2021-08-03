namespace Domain

[<AutoOpen>]
module ParsedDocModels =

    type PassportRF =
        { Issuer: string
          IssuerCode: string
          BirthDate: string
          Number: string
          FirstName: string
          LastName: string
          MiddleName: string
          Sex: string
          IssueDate: string
          BirthPlace: string }

    type ParsedDoc = PassportRF of PassportRF
