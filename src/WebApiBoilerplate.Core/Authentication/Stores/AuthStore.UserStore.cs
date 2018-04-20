using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebApiBoilerplate.DataModel;
using WebApiBoilerplate.Framework.Database;

namespace WebApiBoilerplate.Core.Authentication.Stores
{
    partial class AuthStore: IUserStore<AuthenticatedUser>
    {
        public Task<string> GetUserIdAsync(AuthenticatedUser user, CancellationToken cancellationToken)
        {
            var id = user.Id.ToString(CultureInfo.InvariantCulture);

            return Task.FromResult(id);
        }

        public async Task<string> GetUserNameAsync(AuthenticatedUser user, CancellationToken cancellationToken)
        {
            var dbUser = await GetDbUserAsync(user, cancellationToken).ConfigureAwait(false);
            return dbUser.Login;
        }

        public async Task SetUserNameAsync(AuthenticatedUser user, string userName, CancellationToken cancellationToken)
        {
            var dbUser = await GetDbUserAsync(user, cancellationToken).ConfigureAwait(false);

            dbUser.Login = userName;

            await dbUser.SaveAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<string> GetNormalizedUserNameAsync(AuthenticatedUser user, CancellationToken cancellationToken)
        {
            var username = await GetUserNameAsync(user, cancellationToken).ConfigureAwait(false);

            return username.ToUpperInvariant();
        }

        public Task SetNormalizedUserNameAsync(AuthenticatedUser user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task<AuthenticatedUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
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

        public async Task<AuthenticatedUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var dbUser = await User.FindByLoginAsync(_dbContext, normalizedUserName, cancellationToken).ConfigureAwait(false);

            if (dbUser != null)
            {
                return GetAuthUser(dbUser);
            }

            return null;
        }

        public void Dispose()
        { }

        #region Not Supported

        public Task<IdentityResult> CreateAsync(AuthenticatedUser user, CancellationToken cancellationToken)
        {
            throw new System.NotSupportedException();
        }

        public Task<IdentityResult> UpdateAsync(AuthenticatedUser user, CancellationToken cancellationToken)
        {
            throw new System.NotSupportedException();
        }

        public Task<IdentityResult> DeleteAsync(AuthenticatedUser user, CancellationToken cancellationToken)
        {
            throw new System.NotSupportedException();
        }

        #endregion
    }
}
