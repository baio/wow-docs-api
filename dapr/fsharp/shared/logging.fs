namespace Shared

[<AutoOpen>]
module Logging =

    open Microsoft.Extensions.Configuration
    open Microsoft.AspNetCore.Hosting    
    open System
    open Serilog
    open Serilog.Sinks.Elasticsearch
    open Serilog.Sinks.Seq
    open Serilog.Enrichers.Span
    
    let createSerilogLogger (configuration: IConfiguration) (webHostBuilder: IWebHostBuilder) =
        let elasticHost = configuration.Item("ElasticHost")
        let seqHost = configuration.Item("SeqHost")        
        let mutable logger = 
            LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithSpan()
        if String.IsNullOrEmpty(elasticHost) |> not then
            logger <- logger.WriteTo.Elasticsearch(ElasticsearchSinkOptions(Uri elasticHost))
        if String.IsNullOrEmpty(seqHost) |> not then
            logger <- logger.WriteTo.Seq(seqHost)
        Log.Logger <- logger.CreateLogger()
        webHostBuilder.UseSerilog()
