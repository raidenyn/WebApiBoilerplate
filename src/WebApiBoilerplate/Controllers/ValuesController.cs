using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApiBoilerplate.Core.Services;
using WebApiBoilerplate.Protocol;

namespace WebApiBoilerplate.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IUserRepository _userRepository;

        public ValuesController([NotNull] IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        // GET api/values
        [HttpGet]
        public Task<PagedList<UserInfo>> Get([FromQuery] ListUserRequest request)
        {
            return _userRepository.ListAsync(request);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public Task<ObjectInfo> Post([FromBody] CreateUserRequest request)
        {
            return _userRepository.CreateAsync(request);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
