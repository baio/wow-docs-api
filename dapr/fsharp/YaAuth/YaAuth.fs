module YaAuth

type YaAuthConfig = {
    ServiceAccountId: string
    KeyId: string
    PrivateKeyPath: string
}

module private JWTToken =

    open System
    open System.IO
    open System.Security.Cryptography
    open Jose

    let private YA_IAM_AUDIENCE = "https://iam.api.cloud.yandex.net/iam/v1/tokens"

    let private createRSA (filePath: string) =
        let chars = File.ReadAllText(filePath) |> Seq.toArray
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

        let rsaPriv = createRSA config.PrivateKeyPath

        Jose.JWT.Encode(payload, rsaPriv, JwsAlgorithm.PS256, headers)

let getIAMToken (config: YaAuthConfig) =

    let jwtToken = JWTToken.getJwtToken config
    