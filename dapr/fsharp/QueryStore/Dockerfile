ARG NET_IMAGE=5.0-buster-slim

FROM mcr.microsoft.com/dotnet/aspnet:${NET_IMAGE} AS base
ARG PORT=3000
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:${NET_IMAGE} AS build
WORKDIR /src

COPY ["Libs/Common/FSharp.Dapr/", "Libs/Common/FSharp.Dapr/"]
COPY ["Libs/Vow/Shared/", "Libs/Vow/Shared/"]
COPY ["Libs/Vow/Domain/", "Libs/Vow/Domain/"]
COPY ["QueryStore/Formatted", "QueryStore/Formatted"]
COPY ["QueryStore/", "QueryStore/"]
COPY ["fsharp.sln", "fsharp.sln"]
RUN dotnet restore QueryStore

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app/publish QueryStore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN echo ${PORT}
EXPOSE  ${PORT}
ENV PORT ${PORT}

ENTRYPOINT ["dotnet", "QueryStore.dll"]

