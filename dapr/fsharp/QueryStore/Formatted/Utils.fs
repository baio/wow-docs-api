namespace QueryStore.Formatted

module Utils =

    open System
    open System.Globalization

    let formatDateToISO =
        function
        | null -> null
        | (str: string) ->
            let (f, res) =
                DateTime.TryParseExact(str, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)

            match f with
            | true -> res.ToString("s", CultureInfo.InvariantCulture) + "Z"
            | false -> null
