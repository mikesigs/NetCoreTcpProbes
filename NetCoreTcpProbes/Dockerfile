FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY NetCoreTcpProbes/NetCoreTcpProbes.csproj NetCoreTcpProbes/
RUN dotnet restore NetCoreTcpProbes/NetCoreTcpProbes.csproj
COPY . .
WORKDIR /src/NetCoreTcpProbes
RUN dotnet build NetCoreTcpProbes.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish NetCoreTcpProbes.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NetCoreTcpProbes.dll"]
