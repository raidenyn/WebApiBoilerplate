using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApiBoilerplate.Framework.Utils;
using WebApiBoilerplate.Protocol;

namespace WebApiBoilerplate.Swagger
{
    public class ErrorOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            void AddErrorResponse(HttpStatusCode statusCode, Type type = null)
            {
                operation.Responses.Add(statusCode.ToString("D"),
                    new Response
                    {
                        Description = StringExtensions.AddSpaces(statusCode.ToString("G")),
                        Schema = type != null ? context.SchemaRegistry.GetOrRegister(type) : null
                    });
            }

            AddErrorResponse(HttpStatusCode.BadRequest, typeof(ValidationError));
            AddErrorResponse(HttpStatusCode.InternalServerError, typeof(Error));

            if (context.ApiDescription.ActionDescriptor.FilterDescriptors.Any(f => f.Filter is AuthorizeFilter))
            {
                AddErrorResponse(HttpStatusCode.Forbidden);
                AddErrorResponse(HttpStatusCode.Unauthorized);
            }
        }
    }
}
