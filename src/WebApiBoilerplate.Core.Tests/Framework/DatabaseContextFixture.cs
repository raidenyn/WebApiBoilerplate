using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using WebApiBoilerplate.DataModel;
using WebApiBoilerplate.Framework;

namespace WebApiBoilerplate.Core.Tests.Framework
{
    public sealed class DatabaseContextFixture: IDisposable
    {
        [NotNull]
        private readonly IServiceScope _serviceScope;

        [NotNull]
        public IServiceProvider ServiceProvider { get; private set; }

        [NotNull]
        public WebApiBorilerplateDbContext DbContext { get; private set; }

        public DatabaseContextFixture([NotNull] IServiceScope serviceScope)
        {
            _serviceScope = serviceScope ?? throw new ArgumentNullException(nameof(serviceScope));
            ServiceProvider = serviceScope.ServiceProvider;
            DbContext = ServiceProvider.GetService<WebApiBorilerplateDbContext>();
        }

        public async Task CommitAsync()
        {
            var dbContext = (ITransactionContext)DbContext;
            await dbContext.CommitAsync().ConfigureAwait(false);
        }

        public async Task RollabckAsync()
        {
            var dbContext = (ITransactionContext)DbContext;
            await dbContext.CommitAsync().ConfigureAwait(false);
        }

        void IDisposable.Dispose()
        {
            _serviceScope.Dispose();
        }
    }
}
