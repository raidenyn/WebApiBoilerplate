using System;
using System.Threading.Tasks;
using FluentValidation;
using JetBrains.Annotations;
using WebApiBoilerplate.Core.Authentication;
using WebApiBoilerplate.DataModel;
using WebApiBoilerplate.Framework.Services;
using WebApiBoilerplate.Framework.Validations;

namespace WebApiBoilerplate.Core.Services.Impl
{
    [Service, UsedImplicitly]
    public class AccountService: IAccountService
    {
        private readonly WebApiBorilerplateDbContext _dbContext;
        private readonly UserManager _userManager;

        public AccountService(
            WebApiBorilerplateDbContext dbContext,
            UserManager userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<AuthUser> CreateUserAsync(Protocol.SignUpAccountRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var user = User.Create(_dbContext);

            user.Login = request.Login;
            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            await user.SaveAsync();

            var authenticatedUser = new AuthUser
            {
                Id = user.Id
            };

            var result = await _userManager.CreateAsync(authenticatedUser, request.Password);

            if (!result.Succeeded)
            {
                var validations = result.Errors.ToValidationFailures(
                    (nameof(Protocol.SignUpAccountRequest.Password), "password"), 
                    (nameof(Protocol.SignUpAccountRequest.Email), "email"),
                    (nameof(Protocol.SignUpAccountRequest.Login), "user name"));
                throw new ValidationException("Request validation failed", validations);
            }

            return authenticatedUser;
        }
    }
}
