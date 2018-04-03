namespace WebApiBoilerplate.Framework.Database.Configuration
{
    internal interface INHibernateConfigurationProvider<TDbContext>
        where TDbContext : DbContext<TDbContext>
    {
        NHibernate.Cfg.Configuration GetConfiguration();
    }
}
