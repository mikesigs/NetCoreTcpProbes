FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY samples/CustomHealthChecksExample/CustomHealthChecksExample.csproj samples/CustomHealthChecksExample/
COPY src/TcpHealthChecks/TcpHealthChecks.csproj src/TcpHealthChecks/
RUN dotnet restore samples/CustomHealthChecksExample/CustomHealthChecksExample.csproj
COPY . .
WORKDIR /src/samples/CustomHealthChecksExample
RUN dotnet build CustomHealthChecksExample.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish CustomHealthChecksExample.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CustomHealthChecksExample.dll"]
