using System;
using JetBrains.Annotations;

namespace WebApiBoilerplate.Framework.Database.Configuration
{
    internal class NHibernateConfigurationProvider<TDbContext>: INHibernateConfigurationProvider<TDbContext>
        where TDbContext : DbContext<TDbContext>
    {
        private readonly BaseNHibernateConfiguration<TDbContext> _baseConfiguration;
        private readonly NHibernateConfigurationCache<TDbContext> _cache;

        public NHibernateConfigurationProvider(
            [NotNull] BaseNHibernateConfiguration<TDbContext> baseConfiguration,
            [NotNull] NHibernateConfigurationCache<TDbContext> cache
        )
        {
            _baseConfiguration = baseConfiguration ?? throw new ArgumentNullException(nameof(baseConfiguration));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public NHibernate.Cfg.Configuration GetConfiguration()
        {
            if (_cache.IsValid())
            {
                return _cache.Load();
            }

            var configuration = _baseConfiguration.GetConfiguration();

            _cache.Save(configuration);

            return configuration;
        }
    }
}
