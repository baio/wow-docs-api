namespace Shared

open Microsoft.Extensions.Configuration

[<AutoOpen>]
module Logging =

    open Microsoft.AspNetCore.Hosting    
    open System
    open Serilog
    open Serilog.Sinks.Elasticsearch
    open Serilog.Enrichers.Span
    
    let createSerilogLogger (configuration: IConfiguration) (webHostBuilder: IWebHostBuilder) =
        let elasticHost = configuration.Item("ElasticHost")
        
        let logger = 
            LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithSpan()
                .WriteTo.Elasticsearch(ElasticsearchSinkOptions(Uri elasticHost))
                .CreateLogger()
        Log.Logger <- logger                    
        webHostBuilder.UseSerilog()
