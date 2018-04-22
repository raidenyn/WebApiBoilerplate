using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using WebApiBoilerplate.DataModel;
using WebApiBoilerplate.Framework.Database;

namespace WebApiBoilerplate.Core.Authentication.Stores
{
    public sealed partial class UserStore
    {
        [NotNull]
        private readonly WebApiBorilerplateDbContext _dbContext;

        [NotNull]
        private readonly ILookupNormalizer _keyNormalizer;

        [UsedImplicitly]
        public UserStore(
            [NotNull] WebApiBorilerplateDbContext dbContext, 
            [NotNull] ILookupNormalizer keyNormalizer)
        {
            _dbContext = dbContext;
            _keyNormalizer = keyNormalizer;
        }

        [NotNull, ItemNotNull]
        private Task<User> GetDbUserAsync([NotNull] AuthUser auth, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _dbContext.GetAsync<User>(auth.Id, cancellationToken);
        }

        [NotNull]
        private AuthUser GetAuthUser([NotNull] User user)
        {
            return new AuthUser
            {
                Id = user.Id,
            };
        }

        public void Dispose()
        { }
    }
}
