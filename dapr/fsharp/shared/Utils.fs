namespace Shared

[<AutoOpenAttribute>]
module Utils =
    let getAppPort defaultPort =
        match System.Environment.GetEnvironmentVariable("PORT") with        
        | null -> defaultPort.ToString()
        | port -> port
    
    let getAppUrl defaultPort = $"http://*:{getAppPort defaultPort}"