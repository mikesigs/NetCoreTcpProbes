FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY Example/Example.csproj Example/
RUN dotnet restore Example/Example.csproj
COPY . .
WORKDIR /src/Example
RUN dotnet build Example.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Example.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Example.dll"]
