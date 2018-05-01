using System;
using System.Data.SqlClient;

namespace WebApiBoilerplate.Core.Tests
{
    internal static class TestDatabaseConnectionString
    {
        private const string DockerConnectionString = "Data Source=mssql;User ID=sa;Password=wiEPzF9pXnuVuejTN3p7;";

        private const string WindowConnectionString = "Data Source=localhost,14336;Persist Security Info=True;User ID=sa;Password=wiEPzF9pXnuVuejTN3p7;";


        private static readonly Lazy<string> LazyServerConnectionString = new Lazy<string>(() =>
        {
            var connectionString = Environment.GetEnvironmentVariable("WebApiBoilerplateCoreTestsConnectionString");
            if (connectionString != null)
            {
                return connectionString;
            }

            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                return DockerConnectionString;
            }

            return WindowConnectionString;
        });

        private static readonly Lazy<string> LazyDatabaseName = new Lazy<string>(() =>
        {
            var databaseName = Environment.GetEnvironmentVariable("WebApiBoilerplateCoreTestsDatabaseName");
            if (databaseName != null)
            {
                return databaseName;
            }

            return "WebApiBoilerplate_Test";
        });

        private static readonly Lazy<string> LazyDatabaseConnectionString = new Lazy<string>(() =>
        {
            var builder = new SqlConnectionStringBuilder(ServerConnectionString)
            {
                InitialCatalog = DatabaseName
            };

            return builder.ToString();
        });


        public static string DatabaseName => LazyDatabaseName.Value;

        public static string ServerConnectionString => LazyServerConnectionString.Value;

        public static string DatabaseConnectionString => LazyDatabaseConnectionString.Value;
    }
}
