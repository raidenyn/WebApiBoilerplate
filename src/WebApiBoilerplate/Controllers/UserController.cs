using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using WebApiBoilerplate.Core.Services;
using WebApiBoilerplate.Protocol;

namespace WebApiBoilerplate.Controllers
{
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

        [HttpPost]
        public Task<ObjectInfo> Create([FromBody] CreateUserRequest request)
        {
            return _userRepository.CreateAsync(request);
        }

        [HttpPut("{id:int}")]
        public Task Update([FromBody] UpdateUserRequest request)
        {
            return _userRepository.UpdateAsync(request);
        }

        [HttpDelete("{id:int}")]
        public Task Remove(RemoveUserRequest request)
        {
            return _userRepository.RemoveAsync(request);
        }
    }
}
