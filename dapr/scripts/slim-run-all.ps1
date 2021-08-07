$Env:APP_ENV="slim"
Start-Process powershell {dapr run --app-id read-file --app-port 3010 --components-path ./dapr/slim-components -- dotnet watch run --project fsharp/ReadFile PORT=3010}
Start-Process powershell {dapr run --app-id ya-ocr --app-port 3020 --components-path ./dapr/slim-components -- dotnet watch run --project fsharp/YaOCR  PORT=3020}
Start-Process powershell {dapr run --app-id parse-doc --app-port 3030 --components-path ./dapr/slim-components -- dotnet watch run --project fsharp/ParseDoc  PORT=3030}
Start-Process powershell {dapr run --app-id update-store --app-port 3040 --components-path ./dapr/slim-components -- dotnet watch run --project fsharp/UpdateStore  PORT=3040}
Start-Process powershell {dapr run --app-id query-store --app-port 3050 --components-path ./dapr/slim-components -- dotnet watch run --project fsharp/QueryStore  PORT=3050}

