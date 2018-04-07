using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiBoilerplate.Framework.Web
{
    public class NHibernateTransactionActionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.HttpContext.SetError(context.Exception);
        }
    }
}
