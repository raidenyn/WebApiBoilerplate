using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace WebApiBoilerplate.Framework.Database
{
    public abstract class PersistentClassMapping<TPersistent>: ClassMapping<TPersistent>
         where TPersistent : DbObject
    {
        public const string DefaultSchema = "dbo";

        protected PersistentClassMapping()
        {
            Lazy(true);
            Id(x => x.Id, map =>
            {
                map.Generator(Generators.HighLow, gen => gen.Params(new
                {
                    max_lo = 100, // size of ids range reserved at once - Int16.MaxValue by default
                    table = "IdentityInfo",
                    schema = DefaultSchema,
                    column = "Hi",
                }));
            });
        }
    }
}
