namespace Shared

[<AutoOpen>]
module DaprApp =

    open Dapr.Client
    open Microsoft.Extensions.Configuration
    open Saturn
    open Shared
    open System.Text.Json.Serialization
    open Giraffe
    open System.Text.Json

    let daprApp (defaultAppPort) routes =

        // Make all parts to talk same json 

        // https://giraffe.wiki/docs#json
        // https://github.com/Tarmil/FSharp.SystemTextJson
        // set json converter for dapr https://github.com/dapr/dotnet-sdk/issues/362
        let dapr = DaprClientBuilder().Build()
        let converter = JsonFSharpConverter(JsonUnionEncoding.FSharpLuLike)
        dapr.JsonSerializerOptions.Converters.Add(converter)
        //
        let serializationOptions = JsonSerializerOptions(PropertyNameCaseInsensitive = true)
        serializationOptions.Converters.Add(converter)
        let serializer = SystemTextJson.Serializer(serializationOptions)
        //
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
            use_json_serializer(serializer)
            webhost_config (createSerilogLogger config)   
        }
