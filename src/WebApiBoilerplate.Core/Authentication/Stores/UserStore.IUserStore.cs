using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebApiBoilerplate.DataModel;
using WebApiBoilerplate.Framework.Database;

namespace WebApiBoilerplate.Core.Authentication.Stores
{
    partial class UserStore: IUserStore<AuthUser>
    {
        public Task<string> GetUserIdAsync(AuthUser user, CancellationToken cancellationToken)
        {
            var id = user.Id.ToString(CultureInfo.InvariantCulture);

            return Task.FromResult(id);
        }

        public async Task<string> GetUserNameAsync(AuthUser user, CancellationToken cancellationToken)
        {
            var dbUser = await GetDbUserAsync(user, cancellationToken).ConfigureAwait(false);
            return dbUser.Login;
        }

        public async Task SetUserNameAsync(AuthUser user, string userName, CancellationToken cancellationToken)
        {
            var dbUser = await GetDbUserAsync(user, cancellationToken).ConfigureAwait(false);

            dbUser.Login = userName;

            await dbUser.SaveAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<string> GetNormalizedUserNameAsync(AuthUser user, CancellationToken cancellationToken)
        {
            var username = await GetUserNameAsync(user, cancellationToken).ConfigureAwait(false);

            return _keyNormalizer.Normalize(username);
        }

        public Task SetNormalizedUserNameAsync(AuthUser user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task<AuthUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (long.TryParse(userId, out long id))
            {
                var dbUser = await _dbContext.FindAsync<User>(id, cancellationToken).ConfigureAwait(false);
                if (dbUser != null)
                {
                    return GetAuthUser(dbUser);
                }
            }

            return null;
        }

        public async Task<AuthUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var dbUser = await User.FindByLoginAsync(_dbContext, normalizedUserName, cancellationToken).ConfigureAwait(false);

            if (dbUser != null)
            {
                return GetAuthUser(dbUser);
            }

            return null;
        }

        #region Not Supported

        public Task<IdentityResult> CreateAsync(AuthUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> UpdateAsync(AuthUser user, CancellationToken cancellationToken)
        {
            throw new System.NotSupportedException();
        }

        public Task<IdentityResult> DeleteAsync(AuthUser user, CancellationToken cancellationToken)
        {
            throw new System.NotSupportedException();
        }

        #endregion
    }
}
