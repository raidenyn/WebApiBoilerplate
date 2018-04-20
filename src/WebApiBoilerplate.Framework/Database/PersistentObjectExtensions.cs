using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace WebApiBoilerplate.Framework.Database
{
    public static class PersistentObjectExtensions
    {
        [NotNull]
        public static Task SaveAsync([NotNull] this PersistentObject obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return obj.SaveAsync(false, cancellationToken);
        }

        [NotNull]
        public static Task DeleteAsync([NotNull] this PersistentObject obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return obj.DeleteAsync(false, cancellationToken);
        }
    }
}
