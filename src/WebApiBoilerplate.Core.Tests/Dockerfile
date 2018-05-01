FROM microsoft/dotnet:2.0-sdk

WORKDIR /app
COPY bin/Debug/netcoreapp2.0/publish .

ENTRYPOINT ["dotnet", "WebApiBoilerplate.Core.Tests.dll"]
