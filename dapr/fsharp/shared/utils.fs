namespace Shared

[<AutoOpenAttribute>]
module Utils =
    let getAppPort (defaultPort: int) =
        match System.Environment.GetEnvironmentVariable("PORT") with        
        | null -> defaultPort.ToString()
        | port -> port
    
    let getAppUrl defaultPort = $"http://*:{getAppPort defaultPort}"