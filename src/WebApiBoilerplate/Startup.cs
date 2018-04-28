using System;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(options =>
            {
                options.AddConfiguration(Configuration);
            });

            services.AddIdentity<AuthUser, AuthUserRole>(options =>
                {
                    // Password settings
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredUniqueChars = 6;

                    // Lockout settings
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                    options.Lockout.MaxFailedAccessAttempts = 10;
                    options.Lockout.AllowedForNewUsers = true;

                    // User settings
                    options.User.RequireUniqueEmail = true;
                })
                .AddUserStore<UserStore>()
                .AddRoleStore<RoleStore>()
                .AddUserManager<UserManager>()
                .AddSignInManager<SignInManager>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.Name = "WebApiBoilerplate-auth";
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
                    return Task.CompletedTask;
                };
            });

            services.AddNHibernateDbContext<WebApiBorilerplateDbContext>(config =>
            {
                config.Connection(db =>
                {
                    var server = Environment.OSVersion.Platform == PlatformID.Unix 
                        ? "Data Source=mssql;" 
                        : "Data Source=localhost,14336;";
                    db.ConnectionString = server + @"Initial Catalog=WebApiBoilerplate.Database;Persist Security Info=True;User ID=sa;Password=wiEPzF9pXnuVuejTN3p7;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=10;Encrypt=False;TrustServerCertificate=True";
                    db.Dialect<MsSql2012Dialect>();
                    db.Driver<Sql2008ClientDriver>();
                    db.ConnectionProvider<DriverConnectionProvider>();

                    db.LogSqlInConsole = true;
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
                options.ReportApiVersions = false;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseNHibernateTransactionMiddleware<WebApiBorilerplateDbContext>();

            app.UseMvc();

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
