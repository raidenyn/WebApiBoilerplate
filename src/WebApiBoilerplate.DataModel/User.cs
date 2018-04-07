using WebApiBoilerplate.Framework.Database;

namespace WebApiBoilerplate.DataModel
{
    public class User: PersistentObject
    {
        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public static User Create(WebApiBorilerplateDbContext dbContext)
        {
            var user = Create<User>(dbContext.Session);
            
            return user;
        }
    }
}
