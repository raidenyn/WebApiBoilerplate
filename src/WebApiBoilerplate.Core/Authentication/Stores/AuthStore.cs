using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using WebApiBoilerplate.DataModel;
using WebApiBoilerplate.Framework.Database;

namespace WebApiBoilerplate.Core.Authentication.Stores
{
    public sealed partial class AuthStore
    {
        [NotNull]
        private readonly WebApiBorilerplateDbContext _dbContext;

        public AuthStore([NotNull] WebApiBorilerplateDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [NotNull, ItemNotNull]
        private Task<User> GetDbUserAsync([NotNull] AuthenticatedUser auth, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _dbContext.GetAsync<User>(auth.Id, cancellationToken);
        }

        [NotNull]
        private AuthenticatedUser GetAuthUser([NotNull] User user)
        {
            return new AuthenticatedUser
            {
                Id = user.Id,
            };
        }
    }
}
