using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using WebApiBoilerplate.ActionFilters;
using WebApiBoilerplate.DataModel;
using WebApiBoilerplate.Framework.Database;
using WebApiBoilerplate.Framework.Web;
using WebApiBoilerplate.Framework.Web.Transactions;

namespace WebApiBoilerplate
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(options =>
            {
                options.AddConfiguration(Configuration);
            });

            services.AddNHibernateDbContext<WebApiBorilerplateDbContext>(config =>
            {
                config.Connection(db =>
                {
                    db.ConnectionString = @"Data Source=mssql;Initial Catalog=WebApiBoilerplate.Database;Persist Security Info=True;User ID=sa;Password=wiEPzF9pXnuVuejTN3p7;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=10;Encrypt=False;TrustServerCertificate=True";
                    db.Dialect<MsSql2012Dialect>();
                    db.Driver<Sql2008ClientDriver>();
                    db.ConnectionProvider<DriverConnectionProvider>();
                });
            });

            services.AddMvc(options =>
            {
                options.Filters.Add<NHibernateTransactionActionFilter>();
                options.Filters.Add<ErrorFormmaterFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseNHibernateTransactionMiddleware<WebApiBorilerplateDbContext>();

            app.UseMvc();
        }
    }
}
