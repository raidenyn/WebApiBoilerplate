using JetBrains.Annotations;
using WebApiBoilerplate.Framework.Database;
using WebApiBoilerplate.Protocol;

namespace WebApiBoilerplate.Core.Framework
{
    public static class DbObjectProtocolExtensions
    {
        [ContractAnnotation("null => null; notnull => notnull")]
        public static ObjectInfo ToObjectInfo([CanBeNull] this DbObject dbObject)
        {
            if (dbObject == null)
            {
                return null;
            }

            return new ObjectInfo
            {
                Id = dbObject.Id
            };
        }
    }
}
