using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using SqlDeploy;
using WebApiBoilerplate.DataModel;
using WebApiBoilerplate.Framework.Database;

namespace WebApiBoilerplate.Core.Tests.Framework
{
    public sealed class DatabaseFixture : IDisposable
    {
        private readonly ISqlDatabase _database;

        public IServiceProvider ServiceProvider { get; }

        private DatabaseFixture([NotNull] ISqlDatabase database, [NotNull] IServiceProvider serviceProvider)
        {
            _database = database;
            ServiceProvider = serviceProvider;
        }

        public static async Task<DatabaseFixture> CreateAsync()
        {
            var deployer = new SqlProjectDeployer("Database/WebApiBoilerplate.Database.sqlproj");

            var database = await deployer.RecreateToAsync(
                new SqlConnection(TestDatabaseConnectionString.ServerConnectionString),
                TestDatabaseConnectionString.DatabaseName);

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

                    db.LogSqlInConsole = false;
                });
            });

            services.AddCoreServices();

            return new DatabaseFixture(database, services.BuildServiceProvider()); ;
        }

        public async Task DisposeAsync()
        {
            await _database.DeleteAsync().ConfigureAwait(false);
            _database.Dispose();
        }

        public void Dispose()
        {
            DisposeAsync().Wait();
        }
    }
}
