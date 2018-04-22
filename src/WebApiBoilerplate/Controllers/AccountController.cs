using System.Net;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApiBoilerplate.Core.Authentication;
using WebApiBoilerplate.Core.Services;
using WebApiBoilerplate.Protocol;

namespace WebApiBoilerplate.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/account")]
    public class AccountController: Controller
    {
        [NotNull]
        private readonly UserManager<AuthUser> _userManager;
        
        [NotNull]
        private readonly SignInManager<AuthUser> _signInManager;

        [NotNull]
        private readonly IAccountService _accountService;

        public AccountController(
            [NotNull] UserManager<AuthUser> userManager, 
            [NotNull] SignInManager<AuthUser> signInManager, 
            [NotNull] IAccountService accountService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountService = accountService;
        }

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

        [HttpPost("sign-up")]
        [AllowAnonymous]
        public async Task SignUp([FromBody] SignUpAccountRequest request)
        {
            var authenticatedUser = await _accountService.CreateUserAsync(request);

            await _signInManager.SignInAsync(
                authenticatedUser,
                isPersistent: false,
                authenticationMethod: CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
