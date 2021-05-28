namespace Shared

[<AutoOpen>]
module Logging =

    open System
    open Serilog
    open Serilog.Sinks.Elasticsearch
    open Serilog.Enrichers.Span
    
    let createSerilogLogger () =
        let logger = 
            LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", "app")
                .Enrich.FromLogContext()
                .Enrich.WithSpan()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(ElasticsearchSinkOptions(Uri "http://localhost:9200"))
                .CreateLogger()
        Log.Logger <- logger                    
