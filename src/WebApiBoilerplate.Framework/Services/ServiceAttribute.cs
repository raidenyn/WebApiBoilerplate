using System;
using Microsoft.Extensions.DependencyInjection;

namespace WebApiBoilerplate.Framework.Services
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute
    {
        public ServiceAttribute()
        {
            Lifetime = ServiceLifetime.Scoped;
        }

        public ServiceLifetime Lifetime { get; set; }
    }
}
