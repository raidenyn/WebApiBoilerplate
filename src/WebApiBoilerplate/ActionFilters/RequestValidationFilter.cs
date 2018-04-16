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
                    Code = "validation-fault",
                    Validations = context.ModelState.Root.ToValidationMessages().ToList()
                });
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }
    }

    public static class ModelStateExtensions
    {
        [NotNull]
        public static IEnumerable<string> ToValidationMessages([NotNull] this ModelStateEntry root)
        {
            var nodes = new Stack<ModelStateEntry>();
            nodes.Push(root);

            while (nodes.Count > 0)
            {
                var current = nodes.Pop();

                if (current.Children != null)
                {
                    foreach (var child in current.Children)
                    {
                        nodes.Push(child);
                    }
                }

                if (current.Errors != null)
                {
                    foreach (var error in current.Errors)
                    {
                        yield return error.ErrorMessage;
                    }
                }
            }
        }
    }
}
