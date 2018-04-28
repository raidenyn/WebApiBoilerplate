using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApiBoilerplate.Core.Authentication;
using WebApiBoilerplate.Core.Services;
using WebApiBoilerplate.Protocol;

namespace WebApiBoilerplate.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/account")]
    public class AccountController: Controller
    {
        [NotNull]
        private readonly UserManager _userManager;
        
        [NotNull]
        private readonly SignInManager _signInManager;

        [NotNull]
        private readonly IAccountService _accountService;

        public AccountController(
            [NotNull] UserManager userManager, 
            [NotNull] SignInManager signInManager, 
            [NotNull] IAccountService accountService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountService = accountService;
        }

        /// <summary>
        /// Sign in user with login and password
        /// </summary>
        /// <param name="request">User credentials</param>
        /// <returns>Auth cookies</returns>
        [HttpPost("sign-in")]
        [AllowAnonymous]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.Forbidden, typeof(Error))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, typeof(AccountLockedError))]
        public async Task<IActionResult> SignIn([FromBody] SignInAccountRequest request)
        {
            var result = await _signInManager.PasswordSignInAsync(
                request.Login,
                request.Password,
                request.IsPersistent,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                Trace.Write(User.Identity.IsAuthenticated);
                return NoContent();
            }

            if (result.IsNotAllowed)
            {
                return StatusCode((int)HttpStatusCode.Forbidden, new Error
                {
                    Code = "IsNotAllowed",
                    Message = "Action is not allowed for the user."
                });
            }

            if (result.IsLockedOut)
            {
                var user = await _userManager.FindByNameAsync(request.Login);
                var lockoutDate = await _userManager.GetLockoutEndDateAsync(user);

                return StatusCode((int)HttpStatusCode.Unauthorized, new AccountLockedError
                {
                    Code = "IsLockedOut",
                    Message = "User is blocked.",
                    LockedEndDate = lockoutDate
                });
            }

            return StatusCode((int)HttpStatusCode.Forbidden, new Error
            {
                Code = "Unathorized",
                Message = "Login or password is wrong"
            });
        }

        /// <summary>
        /// Sigon out current user
        /// </summary>
        /// <returns></returns>
        [HttpPost("sign-out")]
        [AllowAnonymous]
        public Task SignOut()
        {
            return _signInManager.SignOutAsync();
        }

        /// <summary>
        /// Sign up a new user
        /// </summary>
        /// <param name="request">New user credentials</param>
        /// <returns>Auth cookies</returns>
        [HttpPost("sign-up")]
        [AllowAnonymous]
        public async Task SignUp([FromBody] SignUpAccountRequest request)
        {
            var authenticatedUser = await _accountService.CreateUserAsync(request);

            await _signInManager.SignInAsync(
                authenticatedUser,
                isPersistent: false);
        }
    }
}
