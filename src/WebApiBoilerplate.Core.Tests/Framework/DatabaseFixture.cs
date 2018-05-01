using System;
using System.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using SqlDeploy;
using WebApiBoilerplate.DataModel;
using WebApiBoilerplate.Framework.Database;

namespace WebApiBoilerplate.Core.Tests.Framework
{
    public class DatabaseFixture : IDisposable
    {
        private readonly ISqlDatabase _database;

        public IServiceProvider ServiceProvider { get; }

        public DatabaseFixture()
        {
            var deployer = new SqlProjectDeployer("Database/WebApiBoilerplate.Database.sqlproj");

            _database = deployer.RecreateToAsync(
                new SqlConnection(TestDatabaseConnectionString.ServerConnectionString),
                TestDatabaseConnectionString.DatabaseName).Result;

            //setup our DI
            var services = new ServiceCollection();

            services.AddNHibernateDbContext<WebApiBorilerplateDbContext>(config =>
            {
                config.Connection(db =>
                {
                    db.ConnectionString = TestDatabaseConnectionString.DatabaseConnectionString;
                    db.Dialect<MsSql2012Dialect>();
                    db.Driver<Sql2008ClientDriver>();
                    db.ConnectionProvider<DriverConnectionProvider>();

                    db.LogSqlInConsole = true;
                });
            });

            services.AddCoreServices();

            ServiceProvider = services.BuildServiceProvider();
        }

        public void Dispose()
        {
            _database.DeleteAsync().Wait();
            _database.Dispose();
        }
    }
}
