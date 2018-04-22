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
        private static readonly PropertyNameComparer PropertyNameComparer = new PropertyNameComparer();

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

            var members = validatorDescriptor.GetMembersWithValidators()
                .ToDictionary(p => p.Key, PropertyNameComparer);

            foreach (var key in model.Properties.Keys)
            {
                if (members.TryGetValue(key, out var validators))
                {
                    foreach (var validatorRule in validators)
                    {
                        if (validatorRule is NotNullValidator ||
                            validatorRule is NotEmptyValidator)
                        {
                            model.Required.Add(key);
                        }
                    }
                }
            }
        }
    }
}
