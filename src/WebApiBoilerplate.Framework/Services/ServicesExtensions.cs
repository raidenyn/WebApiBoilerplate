using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace WebApiBoilerplate.Framework.Services
{
    public static class ServicesExtensions
    {
        public static void AddAssembliesServices([NotNull] this IServiceCollection services, [NotNull] Action<ServicesOptions> configAction)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configAction == null) throw new ArgumentNullException(nameof(configAction));

            var options = new ServicesOptions();
            configAction(options);

            var builder = new ServicesBuilder(services);

            builder.AddFrom(options.Assemblies);
        }
    }

    public class ServicesOptions
    {
        [NotNull]
        public List<Assembly> Assemblies { get; } = new List<Assembly>();
    }
}
