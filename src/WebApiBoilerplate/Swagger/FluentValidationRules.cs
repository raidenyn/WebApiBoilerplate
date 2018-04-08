using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Validators;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApiBoilerplate.Swagger
{
    public class FluentValidationRules : ISchemaFilter
    {
        public void Apply(Schema model, SchemaFilterContext context)
        {
            var validatorType = context.SystemType.GetNestedType("Validator");

            if (validatorType == null)
            {
                return;
            }

            if (!(Activator.CreateInstance(validatorType) is IValidator validator))
            {
                throw new SystemException($"Validator for type '{validatorType}' should implement IValidator.");
            }

            var validatorDescriptor = validator.CreateDescriptor();

            model.Required = model.Required ?? new List<string>();

            foreach (var key in model.Properties.Keys)
            {
                var name = key[0].ToString().ToUpperInvariant() + key.Substring(1);

                foreach (var validatorRule in validatorDescriptor.GetValidatorsForMember(name))
                {
                    if (validatorRule is NotNullValidator)
                    {
                        model.Required.Add(key);
                    }
                }
            }
        }
    }
}
