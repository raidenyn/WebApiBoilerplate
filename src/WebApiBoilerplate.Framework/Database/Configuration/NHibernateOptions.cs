using System;
using System.Transactions;
using JetBrains.Annotations;
using NHibernate.Bytecode;
using NHibernate.Cfg;
using NHibernate.Cfg.Loquacious;
using Environment = NHibernate.Cfg.Environment;

namespace WebApiBoilerplate.Framework.Database.Configuration
{
    public class NHibernateOptions<TDbContext>
        where TDbContext : DbContext<TDbContext>
    {
        public NHibernate.Cfg.Configuration Configuration { get; } = new NHibernate.Cfg.Configuration();

        [UsedImplicitly]
        public NHibernateOptions()
        {
            Environment.UseReflectionOptimizer = true;

            Configuration.Properties[Environment.PrepareSql] = "true";
            Configuration.Properties[Environment.UseProxyValidator] = "false";
            Configuration.Properties[Environment.BatchSize] = "100";
            Configuration.Properties[Environment.CommandTimeout] = "6000";
            Configuration.Properties[Environment.UseSecondLevelCache] = "true";
            Configuration.Properties[Environment.UseQueryCache] = "true";
            //Configuration.Properties[Environment.CacheProvider] = typeof(NHibernate.Caches.SysCache.SysCacheProvider).AssemblyQualifiedName;
            Configuration.Properties[Environment.Isolation] = IsolationLevel.ReadCommitted.ToString();

#if DEBUG
            Configuration.Properties[Environment.ShowSql] = "true";
            Configuration.Properties[Environment.FormatSql] = "true";
            Configuration.Properties[Environment.UseSqlComments] = "true";
            //Configuration.Properties[Environment.GenerateStatistics] = "true";
#endif

            Configuration.Proxy(p => p.ProxyFactoryFactory<DefaultProxyFactoryFactory>());
        }

        public void Connection([NotNull] Action<IDbIntegrationConfigurationProperties> connection)
        {
            Configuration.DataBaseIntegration(connection);
        }
    }
}
