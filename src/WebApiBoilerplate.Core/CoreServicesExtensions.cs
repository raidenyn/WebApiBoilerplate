using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using WebApiBoilerplate.Framework.Services;

namespace WebApiBoilerplate.Core
{
    public static class CoreServicesExtensions
    {
        public static void AddCoreServices([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddAssembliesServices(options =>
            {
                options.Assemblies.Add(typeof(CoreServicesExtensions).Assembly);
            });
        }
    }
}
