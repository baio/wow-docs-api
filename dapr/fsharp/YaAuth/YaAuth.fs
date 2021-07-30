namespace YaAuth

type YaAuthConfig = {
    ServiceAccountId: string
    KeyId: string
    PrivateKey: string
}

module private JWTToken =

    open System
    open System.Security.Cryptography
    open Jose

    let private YA_IAM_AUDIENCE = "https://iam.api.cloud.yandex.net/iam/v1/tokens"

    let private createRSA (text: string) =
        let chars = text |> Seq.toArray
        let span = System.ReadOnlySpan(chars)
        let rsa = RSA.Create()
        rsa.ImportFromPem(span)
        rsa

    let getJwtToken (config: YaAuthConfig) = 
        let now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        let headers = [ ("kid", config.KeyId :> obj); ("type", "JWT" :> obj)] |> Map.ofSeq

        let payload = [
            ("aud", YA_IAM_AUDIENCE :> obj)
            ("iss", config.ServiceAccountId :> obj)
            ("iat", now :> obj)
            ("exp", (now + int64 3600) :> obj) ] |> Map.ofSeq 

        let rsaPriv = createRSA config.PrivateKey

        Jose.JWT.Encode(payload, rsaPriv, JwsAlgorithm.PS256, headers)

[<AutoOpen>]
module YaAuth =
    open FSharp.Control.Tasks
    open FSharp.Fetch

    type Response = {
        iamToken: string
        code: int
        message: string
    }

    let private YA_TOKENS_URL="https://iam.api.cloud.yandex.net/iam/v1/tokens"

    let getIAMToken (config: YaAuthConfig) =

        let jwtToken = JWTToken.getJwtToken config

        let request: Request<_> = {
            Url = YA_TOKENS_URL
            Method = POST
            Body = Some {| jwt = jwtToken |}            
        }

        task {
            let! result = fetch<Response, _> request
            let finalResult = 
                result 
                |> Result.bind(fun x -> 
                   match x.code with
                   | 0 -> Ok x.iamToken
                   | _ -> Error (exn x.message)
                )

            return finalResult
        }
    