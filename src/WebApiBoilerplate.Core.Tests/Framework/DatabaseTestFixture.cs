using System;
using System.Threading.Tasks;
using System.Transactions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace WebApiBoilerplate.Core.Tests.Framework
{
    public sealed class DatabaseTestFixture: IDisposable
    {
        private readonly DatabaseFixture _databaseFixture;
        private readonly TransactionScope _transactionScope;

        public DatabaseContextFixture Current { get; private set; }

        public DatabaseTestFixture([NotNull] DatabaseFixture databaseFixture)
        {
            _databaseFixture = databaseFixture ?? throw new ArgumentNullException(nameof(databaseFixture));
            _transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew, TransactionScopeAsyncFlowOption.Enabled);

            RenewContext();
        }

        private void RenewContext()
        {
            Current = new DatabaseContextFixture(_databaseFixture.ServiceProvider.CreateScope());
        }

        public async Task CommitAndContinueAsync()
        {
            await Current.CommitAsync().ConfigureAwait(false);

            ((IDisposable)Current).Dispose();

            RenewContext();
        }

        public async Task RollbackAndContinueAsync()
        {
            await Current.RollabckAsync().ConfigureAwait(false);

            ((IDisposable)Current).Dispose();

            RenewContext();
        }

        void IDisposable.Dispose()
        {
            _transactionScope.Dispose();
        }
    }
}
