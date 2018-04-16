using System;
using System.Net;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
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
                        Description = statusCode.ToString("G"),
                        Schema = context.SchemaRegistry.GetOrRegister(type)
                    });
            }

            AddErrorResponse(HttpStatusCode.BadRequest, typeof(ValidationError));
            AddErrorResponse(HttpStatusCode.InternalServerError, typeof(Error));
        }
    }
}
