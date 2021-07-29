ARG NET_IMAGE=5.0-buster-slim

FROM mcr.microsoft.com/dotnet/aspnet:${NET_IMAGE} AS base
ARG PORT=5000
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:${NET_IMAGE} AS build
WORKDIR /src

COPY ["FSharp.Dapr/", "FSharp.Dapr/"]
COPY ["Shared/", "Shared/"]
COPY ["Domain/", "Domain/"]
COPY ["ReadFile/", "ReadFile/"]
COPY ["fsharp.sln", "fsharp.sln"]
RUN dotnet restore ReadFile

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app/publish ReadFile

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN echo ${PORT}
EXPOSE  ${PORT}
ENV PORT ${PORT}

ENTRYPOINT ["dotnet", "ReadFile.dll"]

