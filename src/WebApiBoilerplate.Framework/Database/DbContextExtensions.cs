using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace WebApiBoilerplate.Framework.Database
{
    public static class DbContextExtensions
    {
        [NotNull, ItemCanBeNull]
        public static Task<TDbObject> FindAsync<TDbObject>(
            [NotNull] this DbContext dbContext,
            long id, 
            CancellationToken cancellationToken = default(CancellationToken)) where TDbObject : DbObject
        {
            return dbContext.Session.GetAsync<TDbObject>(id, cancellationToken);
        }

        public static async Task<TDbObject> GetAsync<TDbObject>(
            this DbContext dbContext, 
            long id, 
            CancellationToken cancellationToken = default(CancellationToken)) where TDbObject : DbObject
        {
            var obj = await dbContext.FindAsync<TDbObject>(id, cancellationToken).ConfigureAwait(false);

            if (obj == null)
            {
                throw new ObjectNotFoundException(typeof(TDbObject), id);
            }

            return obj;
        }
    }
}
