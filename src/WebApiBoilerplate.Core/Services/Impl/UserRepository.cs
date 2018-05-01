using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using WebApiBoilerplate.Core.Framework;
using WebApiBoilerplate.DataModel;
using WebApiBoilerplate.Framework;
using WebApiBoilerplate.Framework.Services;

namespace WebApiBoilerplate.Core.Services.Impl
{
    [Service, UsedImplicitly]
    public class UserRepository: IUserRepository
    {
        [NotNull]
        private readonly WebApiBorilerplateDbContext _dbContext;

        public UserRepository([NotNull] WebApiBorilerplateDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Task<Protocol.PagedList<Protocol.UserInfo>> ListAsync(Protocol.ListUserRequest request)
        {
            var users = _dbContext.Session.Query<User>();

            return users.ToPagedListAsync(user => new Protocol.UserInfo
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            }, request);
        }

        public async Task<Protocol.User> GetAsync(Protocol.GetUserRequest request)
        {
            var user = await _dbContext.Session.GetAsync<User>(request.Id);

            if (user.RemovedAt != null)
            {
                throw new ObjectNotFoundException(typeof(User), request.Id);
            }

            return new Protocol.User
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreatedAt = user.CreatedAt,
            };
        }

        public async Task<Protocol.ObjectInfo> CreateAsync(Protocol.CreateUserRequest request)
        {
            var user = User.Create(_dbContext);

            user.Login = request.Login;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;

            await user.SaveAsync(withFlush: true);

            return user.ToObjectInfo();
        }

        public async Task UpdateAsync(Protocol.UpdateUserRequest request)
        {
            var user = await _dbContext.Session.GetAsync<User>(request.Id).ConfigureAwait(false);

            user.Login = request.Login;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;

            await user.SaveAsync(withFlush: true).ConfigureAwait(false);
        }

        public Task RemoveAsync(Protocol.RemoveUserRequest request)
        {
            var user = _dbContext.Session.Get<User>(request.Id);

            return user.RemoveAsync(withFlush: true);
        }
    }
}
