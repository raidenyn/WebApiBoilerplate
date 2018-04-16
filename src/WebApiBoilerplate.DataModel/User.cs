using System;
using System.Threading.Tasks;
using WebApiBoilerplate.Framework.Database;

namespace WebApiBoilerplate.DataModel
{
    public class User: PersistentObject
    {
        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual DateTime CreatedAt { get; protected set; }

        public virtual DateTime? RemovedAt { get; protected set; }

        public static User Create(WebApiBorilerplateDbContext dbContext)
        {
            var user = Create<User>(dbContext.Session);

            user.CreatedAt = DateTime.UtcNow;

            return user;
        }

        public Task RemoveAsync(bool withFlush = false)
        {
            RemovedAt = DateTime.UtcNow;

            return SaveAsync(withFlush);
        }
    }
}
