using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiBoilerplate.Framework;
using WebApiBoilerplate.Protocol;

namespace WebApiBoilerplate.ActionFilters
{
    public class ErrorFormmaterFilter: ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case SystemException systemException:
                {
                    context.Result = GetResult(systemException);
                    context.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                }
                    break;
                
                case WebApiBoilerplateException webApiException:
                {
                    context.Result = GetResult(webApiException);
                    context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                }
                    break;

                default:
                {
                    context.Result = new ObjectResult(new Error
                    {
                        Message = context.Exception.Message,
                        Code = "unknown",
                        // Id = // ToDo: generate new on the fly
                    });
                    context.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                }
                    break;
            }

            context.ExceptionHandled = true;
        }

        private ObjectResult GetResult(WebApiBoilerplateException exception)
        {
            return new ObjectResult(GetError(exception));
        }

        private Error GetError(WebApiBoilerplateException exception)
        {
            return new Error
            {
                Id = exception.ExceptionId,
                Code = exception.Code,
                Message = exception.Message
            };
        }
    }
}
