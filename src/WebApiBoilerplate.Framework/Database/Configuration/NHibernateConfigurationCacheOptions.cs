namespace WebApiBoilerplate.Framework.Database.Configuration
{
    public class NHibernateConfigurationCacheOptions<TDbContext>
        where TDbContext : DbContext<TDbContext>
    {
        public string FilePath { get; set; }

        public string DependceOnFile { get; set; }
    }
}
