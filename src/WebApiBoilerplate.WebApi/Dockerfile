FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY WebApiBoilerplate.sln ./
COPY WebApiBoilerplate.WebApi/WebApiBoilerplate.WebApi.csproj WebApiBoilerplate.WebApi/
COPY WebApiBoilerplate.Core/WebApiBoilerplate.Core.csproj WebApiBoilerplate.Core/
COPY WebApiBoilerplate.DataModel/WebApiBoilerplate.DataModel.csproj WebApiBoilerplate.DataModel/
COPY WebApiBoilerplate.Framework/WebApiBoilerplate.Framework.csproj WebApiBoilerplate.Framework/
COPY WebApiBoilerplate.Framework.Web/WebApiBoilerplate.Framework.Web.csproj WebApiBoilerplate.Framework.Web/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/WebApiBoilerplate.WebApi
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebApiBoilerplate.WebApi.dll"]
