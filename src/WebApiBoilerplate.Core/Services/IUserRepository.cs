using System.Threading.Tasks;
using JetBrains.Annotations;
using WebApiBoilerplate.Core.Protocol;
using WebApiBoilerplate.Framework.Protocol;

namespace WebApiBoilerplate.Core.Services
{
    public interface IUserRepository
    {
        [ItemNotNull]
        Task<PagedList<UserInfo>> ListAsync([CanBeNull] ListUserRequest request);

        [ItemNotNull]
        Task<User> GetAsync([NotNull] GetUserRequest request);

        [ItemNotNull]
        Task<ObjectInfo> CreateAsync([NotNull] CreateUserRequest request);

        Task UpdateAsync([NotNull] UpdateUserRequest request);

        Task RemoveAsync([NotNull] RemoveUserRequest request);
    }
}
