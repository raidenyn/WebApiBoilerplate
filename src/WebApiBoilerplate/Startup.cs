using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using Swashbuckle.AspNetCore.Swagger;
using WebApiBoilerplate.ActionFilters;
using WebApiBoilerplate.Controllers;
using WebApiBoilerplate.Core;
using WebApiBoilerplate.Core.Authentication;
using WebApiBoilerplate.Core.Authentication.Stores;
using WebApiBoilerplate.DataModel;
using WebApiBoilerplate.Framework.Database;
using WebApiBoilerplate.Framework.Utils;
using WebApiBoilerplate.Framework.Web.Transactions;
using WebApiBoilerplate.Protocol;
using WebApiBoilerplate.Swagger;

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

            services.AddCoreServices();

            services.AddMvcCore().AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });

            services.AddMvc(options =>
            {
                options.Filters.Add<RequestValidationFilter>();
                options.Filters.Add<NHibernateTransactionActionFilter>();
                options.Filters.Add<ErrorFormmaterFilter>();
            }).AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblyContaining<ObjectInfo>();
                options.ImplicitlyValidateChildProperties = true;
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Title = "WebApiBoilerplate API", 
                    Version = "v1",
                });
                options.OperationFilter<ErrorOperationFilter>();
                options.OperationFilter<SwaggerDefaultValues>();
                options.SchemaFilter<FluentValidationRules>();
                options.DescribeAllEnumsAsStrings();
                options.DescribeAllParametersInCamelCase();
                options.DescribeStringEnumsInCamelCase();
                options.IncludeXmlComments(typeof(ObjectInfo).Assembly.DocumentationXmlPath());
                options.IncludeXmlComments(typeof(UserController).Assembly.DocumentationXmlPath());
            });

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
            });

            services.AddIdentityCore<AuthenticatedUser>(x =>
                {

                }).AddUserStore<AuthStore>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(o =>
            {
                o.DefaultScheme = IdentityConstants.ApplicationScheme;
                o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            }).AddJwtBearer();
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

            app.UseAuthentication();

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiBoilerplate API v1");
                options.EnableFilter();
                options.EnableValidator();
                options.ShowExtensions();
            });
        }
    }
}
