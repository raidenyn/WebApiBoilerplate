using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using NHibernate;
using WebApiBoilerplate.Framework.Database;

namespace WebApiBoilerplate.DataModel
{
    public class WebApiBorilerplateDbContext: DbContext<WebApiBorilerplateDbContext>
    {
        [UsedImplicitly]
        public WebApiBorilerplateDbContext(
            [NotNull] ISession session, 
            [NotNull] ILogger<WebApiBorilerplateDbContext> logger) : base(session, logger)
        {
        }
    }
}
