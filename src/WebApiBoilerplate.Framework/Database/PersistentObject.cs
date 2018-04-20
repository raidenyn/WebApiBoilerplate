using System;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;

namespace WebApiBoilerplate.Framework.Database
{
    /// <summary>
    /// Base class for all database objects with CRUD operations
    /// </summary>
    public abstract class PersistentObject: DbObject
    {
        public bool IsNew => Id == 0;

        protected static T Create<T>(ISession session) where T : DbObject
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            return (T)Create(session, typeof(T));
        }

        protected static DbObject Create(ISession session, Type type)
        {
            var instance = (DbObject)Activator.CreateInstance(type, nonPublic: true);
            instance.Session = session;
            return instance;
        }

        public virtual async Task SaveAsync(bool withFlush = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Session.SaveOrUpdateAsync(this, cancellationToken).ConfigureAwait(false);
            if (withFlush)
            {
                await Session.FlushAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        public virtual async Task DeleteAsync(bool withFlush = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Session.DeleteAsync(this, cancellationToken).ConfigureAwait(false);
            if (withFlush)
            {
                await Session.FlushAsync(cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
