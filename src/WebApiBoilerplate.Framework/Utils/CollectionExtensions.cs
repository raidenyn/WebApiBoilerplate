using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace WebApiBoilerplate.Framework.Utils
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>([NotNull] this ICollection<T> collection, [NotNull] IEnumerable<T> items)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (items == null) throw new ArgumentNullException(nameof(items));
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
    }
}
