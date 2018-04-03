using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using NHibernate;

namespace WebApiBoilerplate.Framework.Database
{
    public abstract class DbContext<TDbContext>: ITransactionContext
        where TDbContext : DbContext<TDbContext>
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
