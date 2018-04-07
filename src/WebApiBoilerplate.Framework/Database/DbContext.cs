using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using NHibernate;

namespace WebApiBoilerplate.Framework.Database
{
    public abstract class DbContext<TDbContext>: ITransactionContext
        where TDbContext : DbContext<TDbContext>
    {
        private readonly ILogger<DbContext<TDbContext>> _logger;

        [NotNull]
        public ISession Session { get; }

        private readonly ITransaction _transaction;

        protected DbContext([NotNull] ISession session, [NotNull] ILogger<DbContext<TDbContext>> logger)
        {
            Session = session ?? throw new ArgumentNullException(nameof(session));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            session.FlushMode = FlushMode.Commit;

            _transaction = session.BeginTransaction();
        }

        private bool IsCommitable => _transaction.IsActive && Session.IsOpen && Session.IsConnected;

        public Task CommitAsync()
        {
            if (IsCommitable)
            {
                return _transaction.CommitAsync();
            }

            return Task.CompletedTask;
        }

        public Task RollbackAsync()
        {
            if (IsCommitable)
            {
                return _transaction.RollbackAsync();
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            if (IsCommitable)
            {
                _transaction.Rollback();
            }
            Session.Dispose();
        }
    }
}
