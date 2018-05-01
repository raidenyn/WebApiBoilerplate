using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WebApiBoilerplate.Core.Services;
using WebApiBoilerplate.Core.Tests.Framework;
using WebApiBoilerplate.Core.Protocol;
using Xunit;

namespace WebApiBoilerplate.Core.Tests
{
    public class UserRepositoriesListTests: DatabaseTests
    {
        public UserRepositoriesListTests(DatabaseFixture fixture) : base(fixture)
        { }

        [Fact]
        public async Task ListUserTest()
        {
            var userRepo = CurrentScope.ServiceProvider.GetService<IUserRepository>();

            var users = await userRepo.ListAsync(new ListUserRequest
            {
                Name = "user"
            });

            Assert.NotNull(users);
            Assert.Empty(users.Items);
            Assert.Equal(0, users.TotalCount);
        }
    }
}
