using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using WebApiBoilerplate.Core.Framework;
using WebApiBoilerplate.DataModel;
using WebApiBoilerplate.Framework.Services;

namespace WebApiBoilerplate.Core.Services.Impl
{
    [Service]
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

        public async Task<Protocol.ObjectInfo> CreateAsync(Protocol.CreateUserRequest request)
        {
            var user = User.Create(_dbContext);

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            await user.SaveAsync(withFlush: true);

            return user.ToObjectInfo();
        }
    }
}
