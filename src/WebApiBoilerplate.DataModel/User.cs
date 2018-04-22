using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using NHibernate.Linq;
using WebApiBoilerplate.Framework.Database;

namespace WebApiBoilerplate.DataModel
{
    public class User: PersistentObject
    {
        public virtual string Login { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string Email { get; set; }

        public virtual bool EmailIsConfirmed { get; set; }

        public virtual string PasswordHash { get; set; }

        public virtual DateTime CreatedAt { get; protected set; }

        public virtual DateTime? RemovedAt { get; protected set; }

        [NotNull]
        public static User Create([NotNull] WebApiBorilerplateDbContext dbContext)
        {
            var user = Create<User>(dbContext.Session);

            user.CreatedAt = DateTime.UtcNow;

            return user;
        }

        [NotNull]
        public Task RemoveAsync(bool withFlush = false)
        {
            RemovedAt = DateTime.UtcNow;

            return SaveAsync(withFlush);
        }

        [NotNull, ItemCanBeNull]
        public static Task<User> FindByLoginAsync(
            [NotNull] WebApiBorilerplateDbContext dbContext, 
            [NotNull] string login,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = from user in dbContext.Session.Query<User>()
                where user.Login == login
                select user;

            query = query.WithOptions(x =>
            {
                x.SetCacheable(true);
            });

            return query.SingleOrDefaultAsync(cancellationToken);
        }

        [NotNull, ItemCanBeNull]
        public static Task<User> FindByEmailAsync(
            [NotNull] WebApiBorilerplateDbContext dbContext,
            [NotNull] string email,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = from user in dbContext.Session.Query<User>()
                where user.Email == email
                select user;

            query = query.WithOptions(x =>
            {
                x.SetCacheable(true);
            });

            return query.SingleOrDefaultAsync(cancellationToken);
        }
    }
}
