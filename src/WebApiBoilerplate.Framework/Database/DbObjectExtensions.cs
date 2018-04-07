using NHibernate;
using NHibernate.Proxy;

namespace WebApiBoilerplate.Framework.Database
{
    public static class DbObjectExtensions
    {
        public static TDbObject Unproxy<TDbObject>(this TDbObject dbObject)
            where TDbObject: DbObject
        {
            if (!NHibernateUtil.IsInitialized(dbObject))
            {
                NHibernateUtil.Initialize(dbObject);
            }

            if (dbObject.IsProxy())
            {
                return (TDbObject)dbObject.Session.GetSessionImplementation().PersistenceContext.Unproxy(dbObject);
            }

            return dbObject;
        }
    }
}
