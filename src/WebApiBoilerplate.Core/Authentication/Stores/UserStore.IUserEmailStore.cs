using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebApiBoilerplate.DataModel;
using WebApiBoilerplate.Framework.Database;

namespace WebApiBoilerplate.Core.Authentication.Stores
{
    partial class UserStore: IUserEmailStore<AuthUser>
    {
        public async Task SetEmailAsync(AuthUser user, string email, CancellationToken cancellationToken)
        {
            var dbUser = await GetDbUserAsync(user, cancellationToken).ConfigureAwait(false);

            dbUser.Email = email;

            await dbUser.SaveAsync(cancellationToken);
        }

        public async Task<string> GetEmailAsync(AuthUser user, CancellationToken cancellationToken)
        {
            var dbUser = await GetDbUserAsync(user, cancellationToken).ConfigureAwait(false);

            return dbUser.Email;
        }

        public async Task<bool> GetEmailConfirmedAsync(AuthUser user, CancellationToken cancellationToken)
        {
            var dbUser = await GetDbUserAsync(user, cancellationToken).ConfigureAwait(false);

            return dbUser.EmailIsConfirmed;
        }

        public async Task SetEmailConfirmedAsync(AuthUser user, bool confirmed, CancellationToken cancellationToken)
        {
            var dbUser = await GetDbUserAsync(user, cancellationToken).ConfigureAwait(false);

            dbUser.EmailIsConfirmed = confirmed;

            await dbUser.SaveAsync(cancellationToken);
        }

        public async Task<AuthUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            var dbUser = await User.FindByEmailAsync(_dbContext, normalizedEmail, cancellationToken).ConfigureAwait(false);

            if (dbUser != null)
            {
                return GetAuthUser(dbUser);
            }

            return null;
        }

        public async Task<string> GetNormalizedEmailAsync(AuthUser user, CancellationToken cancellationToken)
        {
            var dbUser = await GetDbUserAsync(user, cancellationToken).ConfigureAwait(false);

            return _keyNormalizer.Normalize(dbUser.Email);
        }

        public Task SetNormalizedEmailAsync(AuthUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
