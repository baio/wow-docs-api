namespace YaOCR

module YaOCRJson =

    type Vertex = { x: string; y: string }

    type Language =
        { languageCode: string
          confidence: float }

    type BoundingBox = { vertices: Vertex [] }

    type Word =
        { boundingBox: BoundingBox
          languages: Language []
          text: string
          confidence: float }

    type Line =
        { boundingBox: BoundingBox
          words: Word []
          confidence: float }

    type Block =
        { boundingBox: BoundingBox
          lines: Line [] }

    type Page =
        { blocks: Block []
          width: string
          height: string }

    type TextDetection = { pages: Page [] }

    type ResultResult = { textDetection: TextDetection }

    type YaOcrResult = { results: ResultResult [] }

    type YaOcr =
        { results: YaOcrResult []
          code: int
          message: string }



module internal ExtractText =

    open FSharp.Dapr
    open FSharp.Fetch
    open FSharp.Control.Tasks
    open YaOCR.Constants

    exception UnAuthorizedException of string

    let YA_OCR_URL =
        "https://vision.api.cloud.yandex.net/vision/v1/batchAnalyze"

    let private getYaFolder dapr =

        task {

            let! yaFolder = getSecret dapr SECRET_STORE_NAME SECRET_NAME YA_FOLDER_KEY

            let yaFolder =
                match yaFolder with
                | Some (yaFolder) -> yaFolder
                | _ -> raise (exn "YA_FOLDER secrets not found")

            return yaFolder
        }

    let cleanWordsLine words =
        words
        |> Seq.fold
            (fun acc (v: string) ->
                if v.Length = 1 then
                    acc + v
                else
                    acc + " " + v)
            ""
        |> (fun x -> x.ToLower().Trim())

    let extractText dapr iamToken (imgBase64: string) =
        task {
            let! yaFolder = getYaFolder dapr

            let bodyFeatures =
                {| Type = "TEXT_DETECTION"
                   text_detection_config = {| language_codes = [| "*" |] |} |}

            let body =
                {| folderId = yaFolder
                   analyze_specs =
                       [| {| content = imgBase64
                             features = bodyFeatures |} |] |}

            let request =
                { Method = POST
                  Url = YA_OCR_URL
                  Body = Some body
                  Headers = Map [ ("Authorization", $"Bearer {iamToken}") ] }

            printfn "request %O" request

            let! result = fetch<YaOCRJson.YaOcr, _> request

            let wordsResult =
                result
                |> Result.bind
                    (fun yaOcrJson ->
                        match (yaOcrJson.code, yaOcrJson.results) with
                        | 7, _ -> Error(UnAuthorizedException "Unauthorized")
                        | 16, _ -> Error(UnAuthorizedException "Unauthorized")
                        | 0, results ->
                            results.[0].results.[0].textDetection.pages.[0]
                                .blocks
                            |> Array.collect
                                (fun block ->
                                    block.lines
                                    |> Array.collect (fun line -> line.words |> Array.map (fun word -> word.text)))
                                    |> cleanWordsLine
                            |> Ok
                        | _ -> Error(exn "Ya OCR can't extract text"))

            return wordsResult
        }
