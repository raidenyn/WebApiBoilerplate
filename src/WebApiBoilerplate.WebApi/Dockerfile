FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 4586
EXPOSE 44364

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY WebApiBoilerplate.sln ./
COPY WebApiBoilerplate.WebApi/WebApiBoilerplate.WebApi.csproj WebApiBoilerplate.WebApi/
COPY WebApiBoilerplate.Core/WebApiBoilerplate.Core.csproj WebApiBoilerplate.Core/
COPY WebApiBoilerplate.DataModel/WebApiBoilerplate.DataModel.csproj WebApiBoilerplate.DataModel/
COPY WebApiBoilerplate.Framework/WebApiBoilerplate.Framework.csproj WebApiBoilerplate.Framework/
COPY WebApiBoilerplate.Framework.Web/WebApiBoilerplate.Framework.Web.csproj WebApiBoilerplate.Framework.Web/
RUN dotnet restore WebApiBoilerplate.WebApi/WebApiBoilerplate.WebApi.csproj
COPY . .
WORKDIR /src/WebApiBoilerplate.WebApi
RUN dotnet build WebApiBoilerplate.WebApi -c Release -o /app

FROM build AS publish
RUN dotnet publish WebApiBoilerplate.WebApi -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebApiBoilerplate.WebApi.dll"]
