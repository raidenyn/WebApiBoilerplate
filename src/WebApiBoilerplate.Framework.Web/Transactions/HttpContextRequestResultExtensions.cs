using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace WebApiBoilerplate.Framework.Web.Transactions
{
    public static class HttpContextRequestResultExtensions
    {
        private const string HttpContextItemResultName = "HTTP_REQUEST_RESULT";

        public static bool HasError([NotNull] this HttpContext httpContext)
        {
            if (httpContext.Items[HttpContextItemResultName] is HttpRequestResult result)
            {
                return result.HasError;
            }

            return false;
        }

        public static void SetError([NotNull] this HttpContext httpContext, [NotNull] Exception exception)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            httpContext.Items[HttpContextItemResultName] = new HttpRequestResult(exception);
        }
    }

    public class HttpRequestResult
    {
        public HttpRequestResult([CanBeNull] Exception exception)
        {
            Exception = exception;
        }

        public bool HasError => Exception != null;

        [CanBeNull]
        public Exception Exception { get; }
    }
}
