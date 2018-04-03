using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using NHibernate;

namespace WebApiBoilerplate.Framework.Database
{
    public interface IDbContextFactory<out TDbContext>
        where TDbContext : DbContext<TDbContext>
    {
        TDbContext CreateNew();
    }

    internal class DbContextFactory<TDbContext>: IDbContextFactory<TDbContext>
        where TDbContext : DbContext<TDbContext>
    {
        [NotNull] private readonly ISessionFactory _sessionFactory;
        [NotNull] private readonly ILogger<TDbContext> _logger;

        public DbContextFactory(
            [NotNull] ISessionFactory sessionFactory,
            [NotNull] ILogger<TDbContext> logger
        )
        {
            _sessionFactory = sessionFactory ?? throw new ArgumentNullException(nameof(sessionFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public TDbContext CreateNew()
        {
            var session = _sessionFactory.OpenSession();

            return (TDbContext) Activator.CreateInstance(typeof(TDbContext), session, _logger);
        }
    }
}
