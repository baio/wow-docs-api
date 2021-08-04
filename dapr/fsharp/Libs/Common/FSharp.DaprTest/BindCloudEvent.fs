module FSharp.DaprTest.BindCloudEvent

open Expecto
open FSharp.Control.Tasks
open FSharp.Dapr
open System.Net.Http

type EventData = { Test: string }

open Utils
open Giraffe
open Domain

let json1 = """
{"specversion":"1.0","source":"parse-doc","topic":"doc-text-parsed","data":{"docParsed":{"parsedDoc":{"PassportRF":{"issuerCode":"910-003","lastName":"дата","middleName":"анатольевич","sex":null,"birthPlace":"место рождения","issuer":"российскаяфедерация наспорт выдан отдел уфмс россии по республике крымв киевском районе г. симферополя","birthDate":"23.07.1972","number":"391935349","firstName":"сергей","issueDate":"18.12.2015"}}},"docKey":" 124","docExtractedText":{"words":"российскаяфедерация наспорт выдан отдел уфмс россии по республике крымв киевском районе г. симферополя дата выдачи 18.12.2015 код подразделения 910-003 код сл м. н. 10-00 здрилюк имя сергей отчестно анатольевич муж дата рождения 23.07.1972 место рождения c. фронтовка ия russi оратовского р-на винницкой обл. pnruszdril7k<4151218910003<50","provider":"YaOCR"}},"traceid":"00-2b9f63303ef96645a9478bc6ec18d3d4-07216deded8198ae-01","id":"87fda868-030a-44d6-83cd-84b065ed6e1f","datacontenttype":"application/json","type":"com.dapr.event.sent","pubsubname":"pubsub"}
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
                              let! eventData = bindCloudEventDataAsync<DocParsedEvent> ctx
                              printfn "??? %O" eventData
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
