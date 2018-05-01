using System.Threading.Tasks;
using JetBrains.Annotations;
using WebApiBoilerplate.Core.Authentication;
using WebApiBoilerplate.Core.Protocol;

namespace WebApiBoilerplate.Core.Services
{
    public interface IAccountService
    {
        [NotNull, ItemNotNull]
        Task<AuthUser> CreateUserAsync([NotNull] SignUpAccountRequest request);
    }
}
