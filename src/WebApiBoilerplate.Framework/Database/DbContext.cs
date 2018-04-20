using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using NHibernate;

namespace WebApiBoilerplate.Framework.Database
{
    public abstract class DbContext: ITransactionContext
    {
        [NotNull]
        public ISession Session { get; }

        private readonly ITransaction _transaction;

        protected DbContext([NotNull] ISession session)
        {
            Session = session ?? throw new ArgumentNullException(nameof(session));

            session.FlushMode = FlushMode.Commit;

            _transaction = session.BeginTransaction();
        }

        private bool IsCommitable => _transaction.IsActive && Session.IsOpen && Session.IsConnected;

        Task ITransactionContext.CommitAsync()
        {
            if (IsCommitable)
            {
                return _transaction.CommitAsync();
            }

            return Task.CompletedTask;
        }

        Task ITransactionContext.RollbackAsync()
        {
            if (IsCommitable)
            {
                return _transaction.RollbackAsync();
            }

            return Task.CompletedTask;
        }

        void IDisposable.Dispose()
        {
            if (IsCommitable)
            {
                _transaction.Rollback();
            }
            Session.Dispose();
        }
    }

    public abstract class DbContext<TDbContext> : DbContext
        where TDbContext : DbContext<TDbContext>
    {
        protected readonly ILogger<DbContext<TDbContext>> Logger;

        protected DbContext([NotNull] ISession session, [NotNull] ILogger<DbContext<TDbContext>> logger) : base(session)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
