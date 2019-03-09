FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY samples/SimpleExampleWithDefaults/SimpleExampleWithDefaults.csproj samples/SimpleExampleWithDefaults/
COPY src/TcpHealthChecks/TcpHealthChecks.csproj src/TcpHealthChecks/
RUN dotnet restore samples/SimpleExampleWithDefaults/SimpleExampleWithDefaults.csproj
COPY . .
WORKDIR /src/samples/SimpleExampleWithDefaults
RUN dotnet build SimpleExampleWithDefaults.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish SimpleExampleWithDefaults.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SimpleExampleWithDefaults.dll"]
