namespace Shared

[<AutoOpen>]
module FixJsonSerializer =

    
    open Giraffe
    open Newtonsoft.Json.Serialization
    
    open Microsoft.FSharpLu.Json
    open Newtonsoft.Json
    
    open Microsoft.Extensions.DependencyInjection
    
    let getFixedJsonSerializer () = 
        let contractResolver = CamelCasePropertyNamesContractResolver()
        
        let customSettings =
            JsonSerializerSettings(ContractResolver = contractResolver)

        customSettings.Converters.Add(CompactUnionJsonConverter(true))
        
        NewtonsoftJson.Serializer(customSettings) :> Json.ISerializer
        
 
    let configureServices (services: IServiceCollection) = 
    
        let contractResolver = CamelCasePropertyNamesContractResolver()
        
        let customSettings =
            JsonSerializerSettings(ContractResolver = contractResolver)

        customSettings.Converters.Add(CompactUnionJsonConverter(true))
        
        let serializer = NewtonsoftJson.Serializer(customSettings)
        
        services.AddSingleton<Json.ISerializer>(serializer)
        |> ignore
    
    

