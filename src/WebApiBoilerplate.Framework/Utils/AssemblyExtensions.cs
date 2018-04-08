using System;
using System.IO;
using System.Reflection;
using JetBrains.Annotations;

namespace WebApiBoilerplate.Framework.Utils
{
    public static class AssemblyExtensions
    {
        public static string DocumentationXmlPath([NotNull] this Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            return Path.ChangeExtension(assembly.Location, "xml");
        }
    }
}
