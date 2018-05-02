using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using WebApiBoilerplate.Core.Services;
using WebApiBoilerplate.Core.Tests.Framework;
using WebApiBoilerplate.Framework;
using WebApiBoilerplate.Protocol;

namespace WebApiBoilerplate.Core.Tests
{
    public class UserRepositoriesTests: DatabaseTests
    {
        public async Task<ObjectInfo> CreateUserAsync()
        {
            var userRepo = State.Current.ServiceProvider.GetService<IUserRepository>();

            ObjectInfo result = await userRepo.CreateAsync(new CreateUserRequest
            {
                Login = "UserLogin",
                LastName = "Last name",
                FirstName = "First name",
                Email = "email@user.com"
            });

            Assert.NotNull(result);
            Assert.True(result.Id > 0, "Id of the a new object should be greater than 0.");

            await State.Current.CommitAsync();

            return result;
        }

        [Test]
        public Task CreateUserTest()
        {
            return CreateUserAsync();
        }

        [Test]
        public async Task CreateAndGetUserTest()
        {
            var creationResult = await CreateUserAsync();

            await State.CommitAndContinueAsync();

            var userRepo = State.Current.ServiceProvider.GetService<IUserRepository>();

            var user = await userRepo.GetAsync(new GetUserRequest { Id = creationResult.Id });

            Assert.NotNull(user);
            Assert.AreEqual(creationResult.Id, user.Id);
        }

        [Test]
        public async Task CreateAndUpdateAndGetUserTest()
        {
            var creationResult = await CreateUserAsync();

            await State.CommitAndContinueAsync();

            var userRepo = State.Current.ServiceProvider.GetService<IUserRepository>();

            await userRepo.UpdateAsync(new UpdateUserRequest
            {
                Id = creationResult.Id,
                Login = "UserLoginNew",
                LastName = "LastNameNew",
                FirstName = "FirstNameNew",
                Email = "email@user.new"
            });

            await State.CommitAndContinueAsync();

            userRepo = State.Current.ServiceProvider.GetService<IUserRepository>();

            var user = await userRepo.GetAsync(new GetUserRequest { Id = creationResult.Id });

            Assert.NotNull(user);
            Assert.AreEqual(creationResult.Id, user.Id);
            Assert.AreEqual("UserLoginNew", user.Login);
            Assert.AreEqual("LastNameNew", user.LastName);
            Assert.AreEqual("FirstNameNew", user.FirstName);
            Assert.AreEqual("email@user.new", user.Email);
        }

        [Test]
        public async Task CreateAndRemoveAndGetUserTest()
        {
            var creationResult = await CreateUserAsync();

            await State.CommitAndContinueAsync();

            var userRepo = State.Current.ServiceProvider.GetService<IUserRepository>();

            await userRepo.RemoveAsync(new RemoveUserRequest{ Id = creationResult.Id});

            await State.CommitAndContinueAsync();

            Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            {
                await userRepo.GetAsync(new GetUserRequest {Id = creationResult.Id});
            });
        }
    }
}
