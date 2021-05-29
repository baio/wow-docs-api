namespace Shared

[<AutoOpen>]
module DaprApp =

    open Dapr.Client
    open Microsoft.Extensions.Configuration
    open Saturn
    open Shared
    open System.Text.Json.Serialization

    let daprApp (defaultAppPort) routes =

        // set json converter for dapr https://github.com/dapr/dotnet-sdk/issues/362
        // pub / store / resore
        let dapr = DaprClientBuilder().Build()
        let converter = JsonFSharpConverter()
        dapr.JsonSerializerOptions.Converters.Add(converter)

        let config =
            ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build()

        application {
            use_config (fun _ -> config)
            use_router (routes dapr)
            url (getAppUrl defaultAppPort)
            use_gzip
            // set dapr converter for sub and method calls (BindJsonAsync - JsonFSharpConverter is not working)
            // TODO : Make to work with JsonFSharpConverter
            use_json_serializer(getFixedJsonSerializer())
            webhost_config (createSerilogLogger config)   
        }
