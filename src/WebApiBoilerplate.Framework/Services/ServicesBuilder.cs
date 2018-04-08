using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace WebApiBoilerplate.Framework.Services
{
    public class ServicesBuilder
    {
        [NotNull]
        private readonly IServiceCollection _services;

        public ServicesBuilder([NotNull] IServiceCollection services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public void AddFrom([NotNull] IEnumerable<Assembly> assemblies)
        {
            var services = GetServices(assemblies);

            foreach (var service in services)
            {
                var interfaces = GetInterfaces(service.Type);

                foreach (var @interface in interfaces)
                {
                    var descriptor = new ServiceDescriptor(@interface, service.Type, service.ServiceAttribute.Lifetime);
                    
                    _services.Add(descriptor);
                }
            }
        }

        [NotNull]
        private IEnumerable<Service> GetServices([NotNull] IEnumerable<Assembly> assemblies)
        {
            var services = from assembly in assemblies
                           from type in assembly.GetTypes()
                           let serviceAttribute = type.GetCustomAttributes(typeof(ServiceAttribute), false).Cast<ServiceAttribute>().FirstOrDefault()
                           where serviceAttribute != null
                           select new Service { Type = type, ServiceAttribute = serviceAttribute };

            return services;
        }

        [NotNull]
        private IEnumerable<Type> GetInterfaces([NotNull] Type @class)
        {
            return @class.GetInterfaces();
        }

        private struct Service
        {
            [NotNull]
            public Type Type { get; set; }

            [NotNull]
            public ServiceAttribute ServiceAttribute { get; set; }
        }
    }
}
