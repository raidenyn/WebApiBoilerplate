using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApiBoilerplate.Framework.Database.Configuration;

namespace WebApiBoilerplate.Framework.Database
{
    public interface IDbContextConfigurator<TDbContext>
        where TDbContext : DbContext<TDbContext>
    {
        [NotNull]
        IDbContextConfigurator<TDbContext> UseConfigurationCache([NotNull] Action<NHibernateConfigurationCacheOptions<TDbContext>> cache);

        [NotNull]
        IDbContextConfigurator<TDbContext> UseConfiguration([NotNull] Action<NHibernateOptions<TDbContext>> configuration);
    }

    public class DbContextConfigurator<TDbContext>: IDbContextConfigurator<TDbContext>
        where TDbContext : DbContext<TDbContext>
    {
        [NotNull] private readonly IServiceCollection _services;

        public DbContextConfigurator(
            [NotNull] IServiceCollection services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));

            services.AddOptions();
            services.AddLogging();

            services.AddSingleton<BaseNHibernateConfiguration<TDbContext>>();
            services.AddSingleton<NHibernateConfigurationCache<TDbContext>>();
            services.AddSingleton<INHibernateConfigurationProvider<TDbContext>, NHibernateConfigurationProvider<TDbContext>>();

            services.AddSingleton<IDbContextFactory<TDbContext>> (container =>
            {
                var configurationProvider = container.GetRequiredService<INHibernateConfigurationProvider<TDbContext>>();
                var logger = container.GetRequiredService<ILogger<TDbContext>>();

                var configuration = configurationProvider.GetConfiguration();

                var sessionFactory = configuration.BuildSessionFactory();

                return new DbContextFactory<TDbContext>(sessionFactory, logger);
            });

            services.AddScoped(container =>
            {
                var factory = container.GetRequiredService<IDbContextFactory<TDbContext>>();
                return factory.CreateNew();
            });
        }

        public IDbContextConfigurator<TDbContext> UseConfiguration(Action<NHibernateOptions<TDbContext>> configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            _services.Configure(configuration);

            return this;
        }

        public IDbContextConfigurator<TDbContext> UseConfigurationCache(Action<NHibernateConfigurationCacheOptions<TDbContext>> cache)
        {
            if (cache == null) throw new ArgumentNullException(nameof(cache));

            _services.Configure(cache);

            return this;
        }
    }
}
