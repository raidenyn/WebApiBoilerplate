using System;
using System.Collections.Generic;

namespace WebApiBoilerplate.WebApi.Swagger
{
    public class PropertyNameComparer: IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return String.Equals(x, y, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(string obj)
        {
            return obj?.ToLowerInvariant().GetHashCode() ?? 0;
        }
    }
}
