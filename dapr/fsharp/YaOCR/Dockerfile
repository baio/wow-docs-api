ARG NET_IMAGE=5.0-buster-slim

FROM mcr.microsoft.com/dotnet/aspnet:${NET_IMAGE} AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:${NET_IMAGE} AS build
WORKDIR /src

COPY ["Shared/", "Shared/"]
COPY ["Domain/", "Domain/"]
COPY ["YaOCR/", "YaOCR/"]
COPY ["fsharp.sln", "fsharp.sln"]
RUN dotnet restore YaOCR

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app/publish YaOCR

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YaOCR.dll"]

