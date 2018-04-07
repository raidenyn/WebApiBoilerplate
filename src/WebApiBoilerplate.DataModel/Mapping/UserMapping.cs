using WebApiBoilerplate.Framework.Database;

namespace WebApiBoilerplate.DataModel.Mapping
{
    public class UserMapping: DbObjectClassMapping<User>
    {
        public UserMapping()
        {
            Property(x => x.FirstName, m => m.NotNullable(true));
            Property(x => x.LastName, m => m.NotNullable(true));
        }
    }
}
