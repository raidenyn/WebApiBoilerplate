using WebApiBoilerplate.Framework.Database;

namespace WebApiBoilerplate.DataModel.Mapping
{
    public class UserMapping: DbObjectClassMapping<User>
    {
        public UserMapping()
        {
            Property(x => x.Login, m => m.NotNullable(true));
            Property(x => x.FirstName, m => m.NotNullable(false));
            Property(x => x.LastName, m => m.NotNullable(false));
            Property(x => x.PasswordHash, m => m.NotNullable(false));
            Property(x => x.PasswordSolt, m => m.NotNullable(false));
            Property(x => x.CreatedAt, m => m.NotNullable(true));
            Property(x => x.RemovedAt, m => m.NotNullable(false));
        }
    }
}
