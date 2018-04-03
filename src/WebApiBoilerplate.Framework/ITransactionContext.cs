using System;
using System.Threading.Tasks;

namespace WebApiBoilerplate.Framework
{
    public interface ITransactionContext: IDisposable
    {
        Task CommitAsync();

        Task RollbackAsync();
    }
}
