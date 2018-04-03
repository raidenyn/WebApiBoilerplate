using System;
using System.Reflection;

namespace WebApiBoilerplate.Framework.Database
{
    public static class MappingExtensions
    {
        public static Assembly GetMappingsAssembly(this Type type)
        {
            return type.Assembly;
        }

        public static Assembly GetMappingsAssembly<TDbContext>()
        {
            return GetMappingsAssembly(typeof(TDbContext));
        }
    }
}
