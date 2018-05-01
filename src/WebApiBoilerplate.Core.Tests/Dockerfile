FROM microsoft/dotnet:2.0-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.0-sdk AS build
WORKDIR /src
COPY WebApiBoilerplate.sln ./
COPY WebApiBoilerplate.Core.Tests/WebApiBoilerplate.Core.Tests.csproj WebApiBoilerplate.Core.Tests/
COPY WebApiBoilerplate.Core/WebApiBoilerplate.Core.csproj WebApiBoilerplate.Core/
COPY WebApiBoilerplate.DataModel/WebApiBoilerplate.DataModel.csproj WebApiBoilerplate.DataModel/
COPY WebApiBoilerplate.Framework/WebApiBoilerplate.Framework.csproj WebApiBoilerplate.Framework/
COPY WebApiBoilerplate.Protocol/WebApiBoilerplate.Protocol.csproj WebApiBoilerplate.Protocol/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/WebApiBoilerplate.Core.Tests
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebApiBoilerplate.Core.Tests.dll"]
