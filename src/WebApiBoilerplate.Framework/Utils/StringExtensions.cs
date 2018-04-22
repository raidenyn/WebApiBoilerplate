using System;
using System.Linq;
using JetBrains.Annotations;

namespace WebApiBoilerplate.Framework.Utils
{
    public static class StringExtensions
    {
        /// <summary>
        /// Add spaces before capital letters
        /// </summary>
        [ContractAnnotation("null => null; notnull => notnull;")]
        public static string AddSpaces(string str)
        {
            if (String.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            return String.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? " " + x.ToString() : x.ToString()));
        }
    }
}
