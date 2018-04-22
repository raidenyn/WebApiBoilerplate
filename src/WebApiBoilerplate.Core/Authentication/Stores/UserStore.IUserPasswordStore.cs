using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebApiBoilerplate.Framework.Database;

namespace WebApiBoilerplate.Core.Authentication.Stores
{
    partial class UserStore: IUserPasswordStore<AuthUser>
    {
        public async Task SetPasswordHashAsync(AuthUser user, string passwordHash, CancellationToken cancellationToken)
        {
            var dbUser = await GetDbUserAsync(user, cancellationToken).ConfigureAwait(false);

            dbUser.PasswordHash = passwordHash;

            await dbUser.SaveAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<string> GetPasswordHashAsync(AuthUser user, CancellationToken cancellationToken)
        {
            var dbUser = await GetDbUserAsync(user, cancellationToken).ConfigureAwait(false);

            return dbUser.PasswordHash;
        }

        public async Task<bool> HasPasswordAsync(AuthUser user, CancellationToken cancellationToken)
        {
            var dbUser =  await GetDbUserAsync(user, cancellationToken).ConfigureAwait(false);

            return !String.IsNullOrWhiteSpace(dbUser.PasswordHash);
        }
    }
}
