using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using WebApiBoilerplate.Framework.Database;

namespace WebApiBoilerplate.Framework.Web
{
    public static class NHibernateTransactionMiddlewareExtension
    {
        [NotNull]
        public static IApplicationBuilder UseNHibernateTransactionMiddleware<TDbContext>(
            [NotNull]  this IApplicationBuilder builder)
            where TDbContext : DbContext<TDbContext>
        {
            return builder.UseMiddleware<TransactionContextMiddleware<TDbContext>>();
        }
    }
}
