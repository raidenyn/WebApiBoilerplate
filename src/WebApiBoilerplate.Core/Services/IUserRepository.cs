using System.Threading.Tasks;
using JetBrains.Annotations;
using WebApiBoilerplate.Protocol;

namespace WebApiBoilerplate.Core.Services
{
    public interface IUserRepository
    {
        [ItemNotNull]
        Task<PagedList<UserInfo>> ListAsync([CanBeNull] ListUserRequest request);

        [ItemNotNull]
        Task<ObjectInfo> CreateAsync([NotNull] CreateUserRequest request);
    }
}
