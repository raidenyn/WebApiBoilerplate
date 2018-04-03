using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using WebApiBoilerplate.Framework.Database.Configuration;

namespace WebApiBoilerplate.Framework.Database
{
    public static class NHibernateConfigurationExtension
    {
        [NotNull]
        public static IDbContextConfigurator<TDbContext> AddNHibernateDbContext<TDbContext>(
            [NotNull] this IServiceCollection services,
            [NotNull] Action<NHibernateOptions<TDbContext>> configuration)
            where TDbContext : DbContext<TDbContext>
        {
            return new DbContextConfigurator<TDbContext>(services).UseConfiguration(configuration);
        }
    }
}
