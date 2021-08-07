namespace FSharp.Dapr

[<AutoOpen>]
module private AppUtils =
    open Microsoft.Extensions.Configuration

    let getAppPort (config: IConfigurationRoot) (defaultPort: int) =
        match config.GetValue("PORT") with
        | null -> defaultPort.ToString()
        | port -> port

    let getAppUrl (config: IConfigurationRoot) defaultPort =
        $"http://*:{getAppPort config defaultPort}"
