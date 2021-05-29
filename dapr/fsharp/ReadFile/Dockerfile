FROM mcr.microsoft.com/dotnet/sdk:${NET_IMAGE} AS build
WORKDIR /src

COPY ["ReadFile/", "/ReadFile/"]
COPY ["fsharp.sln", "fsharp.sln"]
RUN dotnet restore

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app/publish

