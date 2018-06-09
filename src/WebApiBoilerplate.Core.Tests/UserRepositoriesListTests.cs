using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using WebApiBoilerplate.Core.Services;
using WebApiBoilerplate.Core.Tests.Framework;
using WebApiBoilerplate.Core.Protocol;

namespace WebApiBoilerplate.Core.Tests
{
    public class UserRepositoriesListTests: DatabaseTests
    {
        [Test]
        public async Task ListUserTest()
        {
            var userRepo = State.Current.ServiceProvider.GetService<IUserRepository>();

            var users = await userRepo.ListAsync(new ListUserRequest
            {
                Name = "user"
            });

            Assert.NotNull(users);
            Assert.IsEmpty(users.Items);
            Assert.AreEqual(0, users.TotalCount);
        }
    }
}
