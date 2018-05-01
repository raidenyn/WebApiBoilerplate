using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WebApiBoilerplate.Core.Services;
using WebApiBoilerplate.Core.Tests.Framework;
using WebApiBoilerplate.Framework;
using WebApiBoilerplate.Core.Protocol;
using WebApiBoilerplate.Framework.Protocol;
using Xunit;

namespace WebApiBoilerplate.Core.Tests
{
    public class UserRepositoriesTests: DatabaseTests
    {
        public UserRepositoriesTests(DatabaseFixture fixture) : base(fixture)
        { }

        [Fact]
        public async Task<ObjectInfo> CreateUserTest()
        {
            var userRepo = CurrentScope.ServiceProvider.GetService<IUserRepository>();

            ObjectInfo result = await userRepo.CreateAsync(new CreateUserRequest
            {
                Login = "UserLogin",
                LastName = "Last name",
                FirstName = "First name",
                Email = "email@user.com"
            });

            Assert.NotNull(result);
            Assert.True(result.Id > 0, "Id of the a new object should be greater than 0.");

            await CommitAsync();

            return result;
        }

        [Fact]
        public async Task CreateAndGetUserTest()
        {
            var creationResult = await CreateUserTest();

            await CommitAndContinueAsync();

            var userRepo = CurrentScope.ServiceProvider.GetService<IUserRepository>();

            var user = await userRepo.GetAsync(new GetUserRequest { Id = creationResult.Id });

            Assert.NotNull(user);
            Assert.Equal(creationResult.Id, user.Id);
        }

        [Fact]
        public async Task CreateAndUpdateAndGetUserTest()
        {
            var creationResult = await CreateUserTest();

            await CommitAndContinueAsync();

            var userRepo = CurrentScope.ServiceProvider.GetService<IUserRepository>();

            await userRepo.UpdateAsync(new UpdateUserRequest
            {
                Id = creationResult.Id,
                Login = "UserLoginNew",
                LastName = "LastNameNew",
                FirstName = "FirstNameNew",
                Email = "email@user.new"
            });

            await CommitAndContinueAsync();

            userRepo = CurrentScope.ServiceProvider.GetService<IUserRepository>();

            var user = await userRepo.GetAsync(new GetUserRequest { Id = creationResult.Id });

            Assert.NotNull(user);
            Assert.Equal(creationResult.Id, user.Id);
            Assert.Equal("UserLoginNew", user.Login);
            Assert.Equal("LastNameNew", user.LastName);
            Assert.Equal("FirstNameNew", user.FirstName);
            Assert.Equal("email@user.new", user.Email);
        }

        [Fact]
        public async Task CreateAndRemoveAndGetUserTest()
        {
            var creationResult = await CreateUserTest();

            await CommitAndContinueAsync();

            var userRepo = CurrentScope.ServiceProvider.GetService<IUserRepository>();

            await userRepo.RemoveAsync(new RemoveUserRequest{ Id = creationResult.Id});

            await CommitAndContinueAsync();

            await Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            {
                await userRepo.GetAsync(new GetUserRequest {Id = creationResult.Id});
            });
        }
    }
}
