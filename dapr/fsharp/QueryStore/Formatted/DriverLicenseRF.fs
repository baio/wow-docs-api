namespace QueryStore.Formatted

[<RequireQualifiedAccess>]
module DriverLicenseRF =

    open Domain
    open Utils

    type DriverLicenseRfDto =
        { Kind: string
          LastName: string
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

    let format (driverLicenseRF: DriverLicenseRF) : DriverLicenseRfDto =
        { Kind = "driver-license-rf"
          LastName = driverLicenseRF.LastName
          LastNameEn = driverLicenseRF.LastNameEn
          FirstName = driverLicenseRF.FirstName
          FirstNameEn = driverLicenseRF.FirstNameEn
          MiddleName = driverLicenseRF.MiddleName
          MiddleNameEn = driverLicenseRF.MiddleNameEn
          Identifier = driverLicenseRF.Identifier
          Issuer = driverLicenseRF.Issuer
          IssuerEn = driverLicenseRF.IssuerEn
          IssueDate = formatDateToISO driverLicenseRF.IssueDate
          ExpiryDate = formatDateToISO driverLicenseRF.ExpiryDate
          DateOfBirth = formatDateToISO driverLicenseRF.DateOfBirth
          RegionOfBirth = driverLicenseRF.RegionOfBirth
          RegionOfBirthEn = driverLicenseRF.RegionOfBirthEn
          IssuerRegion = driverLicenseRF.IssuerRegion
          IssuerRegionEn = driverLicenseRF.IssuerRegionEn
          Categories = driverLicenseRF.Categories }
