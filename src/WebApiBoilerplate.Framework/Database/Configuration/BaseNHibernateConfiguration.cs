using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.Event;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;

namespace WebApiBoilerplate.Framework.Database.Configuration
{
    internal class BaseNHibernateConfiguration<TDbContext>
        where TDbContext : DbContext<TDbContext>
    {
        private readonly NHibernateOptions<TDbContext> _options;

        [UsedImplicitly]
        public BaseNHibernateConfiguration([NotNull] IOptions<NHibernateOptions<TDbContext>> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public NHibernate.Cfg.Configuration GetConfiguration()
        {
            var configuration = _options.Configuration;

            BindEventListeners(configuration);

            SetMappings(configuration);

            return configuration;
        }

        public static void BindEventListeners(NHibernate.Cfg.Configuration configuration)
        {
            var sessionSetter = new DbObject.SetSessionToObject();

            configuration.EventListeners.PostLoadEventListeners = new IPostLoadEventListener[] { sessionSetter };
            configuration.EventListeners.LoadEventListeners = configuration.EventListeners.LoadEventListeners.Union(new ILoadEventListener[] { sessionSetter }).ToArray();
            configuration.EventListeners.PersistEventListeners = new IPersistEventListener[] { sessionSetter };

            configuration.SetInterceptor(new EmptyInterceptor());
        }

        private void SetMappings(NHibernate.Cfg.Configuration configuration)
        {
            var mapper = new ModelMapper();

            mapper.AddMappings(MappingExtensions.GetMappingsAssembly<TDbContext>().GetTypes());

            var mappings = mapper.CompileMappingForAllExplicitlyAddedEntities();

            configuration.AddMapping(mappings);

            SchemaMetadataUpdater.QuoteTableAndColumns(configuration, new MsSql2012Dialect());
        }
    }
}
