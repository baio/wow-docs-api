namespace FSharp.Dapr

[<AutoOpen>]
module private AppUtils =
    let getAppPort (defaultPort: int) =
        match System.Environment.GetEnvironmentVariable("PORT") with        
        | null -> defaultPort.ToString()
        | port -> port
    
    let getAppUrl defaultPort = $"http://*:{getAppPort defaultPort}"