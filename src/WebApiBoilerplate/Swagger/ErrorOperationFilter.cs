using System;
using System.Net;
using System.Text.RegularExpressions;
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
            void AddErrorResponse(HttpStatusCode statusCode, Type type)
            {
                operation.Responses.Add(statusCode.ToString("D"),
                    new Response
                    {
                        Description = StringExtensions.AddSpaces(statusCode.ToString("G")),
                        Schema = context.SchemaRegistry.GetOrRegister(type)
                    });
            }

            AddErrorResponse(HttpStatusCode.BadRequest, typeof(ValidationError));
            AddErrorResponse(HttpStatusCode.InternalServerError, typeof(Error));
        }
    }
}
