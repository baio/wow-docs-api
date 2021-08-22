namespace QueryStore.Formatted

[<AutoOpen>]
module FormatDoc =

    open Domain

    let formatDoc =
        function
        | PassportRF doc -> PassportRF.format doc :> obj
        | ForeignPassportRF doc -> ForeignPassportRF.format doc :> obj
        | DriverLicenseRF doc -> DriverLicenseRF.format doc :> obj
        | _ -> null :> obj
