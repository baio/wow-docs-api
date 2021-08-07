$Env:APP_ENV="slim"
Start-Process powershell {dapr run --app-id read-file --app-port 3001 --components-path ./dapr/slim-components -- dotnet watch run --project fsharp/ReadFile}
Start-Process powershell {dapr run --app-id ya-ocr --app-port 3002 --components-path ./dapr/slim-components -- dotnet watch run --project fsharp/YaOCR}

