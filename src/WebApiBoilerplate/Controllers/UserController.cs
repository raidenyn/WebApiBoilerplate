using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiBoilerplate.Core.Services;
using WebApiBoilerplate.Framework.Web.Extensions;
using WebApiBoilerplate.Protocol;

namespace WebApiBoilerplate.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController([NotNull] IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        /// <summary>
        /// Returns current user data
        /// </summary>
        [HttpGet("current")]
        public Task<User> Current()
        {
            var userId = User.GetUserId();

            if (!userId.HasValue)
            {
                throw new AuthenticationException();
            }

            return _userRepository.GetAsync(new GetUserRequest { Id = userId.Value });
        }

        /// <summary>
        /// Returns list of users with filtering 
        /// </summary>
        [HttpGet]
        public Task<PagedList<UserInfo>> List([FromQuery] ListUserRequest request)
        {
            return _userRepository.ListAsync(request);
        }

        /// <summary>
        /// Returns full data of the user by ID
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public Task<User> Get(GetUserRequest request)
        {
            return _userRepository.GetAsync(request);
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="request">New user data</param>
        /// <returns>ID of the created user</returns>
        [HttpPost]
        public Task<ObjectInfo> Create([FromBody] CreateUserRequest request)
        {
            return _userRepository.CreateAsync(request);
        }

        /// <summary>
        /// Update a exists user
        /// </summary>
        /// <param name="request">New data of the exists user</param>
        [HttpPut("{id:int}")]
        public Task Update([FromBody] UpdateUserRequest request)
        {
            return _userRepository.UpdateAsync(request);
        }

        /// <summary>
        /// Mark a user as removed
        /// </summary>
        /// <param name="request">ID and options of removing user</param>
        [HttpDelete("{id:int}")]
        public Task Remove(RemoveUserRequest request)
        {
            return _userRepository.RemoveAsync(request);
        }
    }
}
