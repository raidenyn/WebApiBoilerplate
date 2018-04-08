using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApiBoilerplate.Framework.Database;

namespace WebApiBoilerplate.Framework.Web.Transactions
{
    public class TransactionContextMiddleware<TDbContext>
        where TDbContext : DbContext<TDbContext>
    {
        [NotNull]
        private readonly RequestDelegate _next;

        public TransactionContextMiddleware([NotNull] RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync([NotNull] HttpContext context)
        {
            var logger = context.RequestServices.GetService<ILogger<TransactionContextMiddleware<TDbContext>>>();
            var dbContext = (ITransactionContext) context.RequestServices.GetService<TDbContext>();

            try
            {
                await _next(context).ConfigureAwait(false);

                if (context.HasError())
                {
                    await dbContext.RollbackAsync().ConfigureAwait(false);
                }
                else
                {
                    await dbContext.CommitAsync().ConfigureAwait(false);
                }
            }
            catch (Exception exception)
            {
                await dbContext.RollbackAsync().ConfigureAwait(false);
                logger.LogError(exception, "Exception on transaction context level");
                throw;
            }
        }
    }
}
