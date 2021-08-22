module FSharp.DaprTest.BindCloudEvent

open Expecto
open FSharp.Control.Tasks
open FSharp.Dapr
open System.Net.Http

type EventData = { Test: string }

open Utils
open Giraffe

type TestData = { Test: string }

let json1 = """
{"specversion":"1.0","source":"parse-doc","topic":"doc-text-parsed","data":{"test": "xxx"},"traceid":"00-2b9f63303ef96645a9478bc6ec18d3d4-07216deded8198ae-01","id":"87fda868-030a-44d6-83cd-84b065ed6e1f","datacontenttype":"application/json","type":"com.dapr.event.sent","pubsubname":"pubsub"}
"""

[<Tests>]
let tests =
    testList
        "BindCloudEvent"
        [ testCase
              "bind cloud event data with null field"
              (fun _ ->


                  let handler: HttpHandler =
                      fun next ctx ->
                          task {
                              let! eventData = bindCloudEventDataAsync<TestData> ctx
                              Expect.equal true true "unexpected result"
                              return! next ctx
                          }

                  let json =
                      """
                    {   "Id": "123",
                        "SpecVersion": "1",
                        "Source": "source",
                        "Type": "type",
                        "DataContentType": "DataContentType",
                        "PubSubName": "pubsub",
                        "TraceId": "1111",
                        "Topic": "test",
                        "Data": { "test" : null }
                    }
                """

                  let httpRequest =
                      new HttpRequestMessage(HttpMethod.Post, "/test")

                  httpRequest.Content <- new StringContent(json1, System.Text.Encoding.UTF8, "application/json")
                  testRequest handler httpRequest |> ignore

                  ) ]
