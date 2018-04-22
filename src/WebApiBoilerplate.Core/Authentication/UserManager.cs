using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebApiBoilerplate.Core.Authentication
{
    public class UserManager: UserManager<AuthUser>
    {
        [UsedImplicitly]
        public UserManager(
            IUserStore<AuthUser> store, 
            IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<AuthUser> passwordHasher, 
            IEnumerable<IUserValidator<AuthUser>> userValidators, 
            IEnumerable<IPasswordValidator<AuthUser>> passwordValidators, 
            ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, 
            IServiceProvider services, 
            ILogger<UserManager<AuthUser>> logger) 
            : base(
                store, 
                optionsAccessor, 
                passwordHasher, 
                userValidators, 
                passwordValidators, 
                keyNormalizer, 
                errors, 
                services, 
                logger)
        { }
    }
}
