using System;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiBoilerplate.Framework;
using WebApiBoilerplate.Protocol;
using SystemException = WebApiBoilerplate.Framework.SystemException;

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
                    context.Result = new ObjectResult(GetError(systemException))
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError
                    };
                }
                    break;
                
                case WebApiBoilerplateException webApiException:
                {
                    context.Result = new ObjectResult(GetError(webApiException))
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest
                    };
                }
                    break;

                case ValidationException validationException:
                {
                    context.Result = new ObjectResult(GetError(validationException, "validation-failed"))
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest
                    };
                }
                    break;

                case AuthenticationException authenticationException:
                {
                    context.Result = new ObjectResult(GetError(authenticationException, "authentication-failed"))
                    {
                        StatusCode = (int)HttpStatusCode.Forbidden
                    };
                }
                    break;

                default:
                {
                    context.Result = new ObjectResult(GetError(context.Exception, "unknown"))
                    {
                        StatusCode = (int) HttpStatusCode.InternalServerError
                    };
                }
                    break;
            }
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

        private Error GetError(Exception exception, string code)
        {
            return new Error
            {
                Id = WebApiBoilerplateException.GetNewId(),
                Code = code,
                Message = exception.Message
            };
        }

        private Error GetError(ValidationException exception, string code)
        {
            return new ValidationError
            {
                Id = WebApiBoilerplateException.GetNewId(),
                Code = code,
                Message = exception.Message,
                Validations = exception.Errors.Select(error => new ValidationFieldError
                {
                    Description = error.ErrorMessage,
                    Field = error.PropertyName,
                }).ToList()
            };
        }
    }
}
