using System.Collections.Generic;
using System.Linq;
using System.Net;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApiBoilerplate.Protocol;

namespace WebApiBoilerplate.ActionFilters
{
    public class RequestValidationFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ObjectResult(new ValidationError
                {
                    Message = "Request contains invalid data. See validation error list.",
                    Code = "validation-failed",
                    Validations = context.ModelState.ToValidationMessages().ToList()
                });
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }
    }

    public static class ModelStateExtensions
    {
        [NotNull]
        public static IEnumerable<ValidationFieldError> ToValidationMessages([NotNull] this ModelStateDictionary modelState)
        {
            foreach (var key in modelState.Keys)
            {
                var state = modelState[key];

                if (state.Errors != null)
                {
                    foreach (var error in state.Errors)
                    {
                        yield return new ValidationFieldError
                        {
                            Description = error.ErrorMessage,
                            Field = key
                        };
                    }
                }
            }
        }
    }
}
