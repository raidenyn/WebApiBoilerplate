using System;
using System.Threading.Tasks;
using System.Transactions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using WebApiBoilerplate.DataModel;
using WebApiBoilerplate.Framework;
using Xunit;

namespace WebApiBoilerplate.Core.Tests.Framework
{
    [Collection(DatabaseCollection.Name)]
    public abstract class DatabaseTests : IDisposable
    {
        private readonly DatabaseFixture _fixture;
        private readonly TransactionScope _transactionScope;

        protected IServiceScope CurrentScope { get; private set; }

        protected WebApiBorilerplateDbContext CurrentDbContext { get; private set; }

        protected DatabaseTests([NotNull] DatabaseFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew, TransactionScopeAsyncFlowOption.Enabled);
            RenewContext();
        }

        private void RenewContext()
        {
            CurrentScope = _fixture.ServiceProvider.CreateScope();
            CurrentDbContext = CurrentScope.ServiceProvider.GetService<WebApiBorilerplateDbContext>();
        }

        protected async Task CommitAsync()
        {
            var dbContext = (ITransactionContext)CurrentDbContext;
            await dbContext.CommitAsync().ConfigureAwait(false);
        }

        protected async Task CommitAndContinueAsync()
        {
            var dbContext = (ITransactionContext)CurrentDbContext;
            await dbContext.CommitAsync().ConfigureAwait(false);

            CurrentScope.Dispose();

            RenewContext();
        }

        protected async Task RollbackAndContinueAsync()
        {
            var dbContext = (ITransactionContext)CurrentDbContext;
            await dbContext.RollbackAsync().ConfigureAwait(false);

            CurrentScope.Dispose();

            RenewContext();
        }

        public void Dispose()
        {
            CurrentScope.Dispose();
            _transactionScope.Dispose();
        }
    }
}
