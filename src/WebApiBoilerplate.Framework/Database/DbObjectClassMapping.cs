using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace WebApiBoilerplate.Framework.Database
{
    /// <summary>
    /// Base class for all mappings
    /// </summary>
    /// <typeparam name="TPersistent"></typeparam>
    public abstract class DbObjectClassMapping<TPersistent>: ClassMapping<TPersistent>
         where TPersistent : DbObject
    {
        public const string DefaultSchema = "dbo";

        protected DbObjectClassMapping()
        {
            // Enable lazy loading by default for all objects
            Lazy(true);

            // Generating objects ids from be HighLow algrithm
            Id(x => x.Id, map =>
            {
                map.Generator(Generators.HighLow, gen => gen.Params(new
                {
                    max_lo = 100, // size of ids range reserved at once - Int16.MaxValue by default
                    table = "IdentityInfo", // Table name (should be exists in DB Schema)
                    schema = DefaultSchema,
                    column = "Hi",
                }));
            });
        }
    }
}
