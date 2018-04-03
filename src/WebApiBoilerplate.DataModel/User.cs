using WebApiBoilerplate.Framework.Database;

namespace WebApiBoilerplate.DataModel
{
    public class User: PersistentObject
    {
        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }
    }
}
