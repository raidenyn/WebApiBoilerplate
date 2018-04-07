using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using NHibernate.Linq;
using WebApiBoilerplate.DataModel;

namespace WebApiBoilerplate.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly WebApiBorilerplateDbContext _dbContext;

        public ValuesController([NotNull] WebApiBorilerplateDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            var users = _dbContext.Session.Query<User>().Select(u => u.FirstName);

            return await users.ToListAsync();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task<long> Post([FromBody]string name)
        {
            var user = DataModel.User.Create(_dbContext);

            user.FirstName = name;
            user.LastName = name;

            await user.SaveAsync();

            return user.Id;
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
