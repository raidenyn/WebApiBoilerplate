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
            AddErrorResponse(HttpStatusCode.BadRequest, operation, context);
            AddErrorResponse(HttpStatusCode.InternalServerError, operation, context);
        }

        private void AddErrorResponse(HttpStatusCode statusCode, Operation operation, OperationFilterContext context)
        {
            operation.Responses.Add(statusCode.ToString("D"),
                new Response
                {
                    Description = statusCode.ToString("G"),
                    Schema = context.SchemaRegistry.GetOrRegister(typeof(Error))
                });
        }
    }
}
