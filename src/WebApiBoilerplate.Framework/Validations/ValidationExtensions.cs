using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;

namespace WebApiBoilerplate.Framework.Validations
{
    public static class ValidationExtensions
    {
        [NotNull]
        public static ValidationFailure ToValidationFailure(
            [NotNull] this IdentityError error, 
            [NotNull] string propertyName)
        {
            if (error == null) throw new ArgumentNullException(nameof(error));
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

            return new ValidationFailure(propertyName, error.Description);
        }

        [NotNull, ItemNotNull]
        public static IEnumerable<ValidationFailure> ToValidationFailures(
            [NotNull, ItemNotNull] this IEnumerable<IdentityError> errors, 
            [NotNull] string propertyName)
        {
            if (errors == null) throw new ArgumentNullException(nameof(errors));
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

            return errors.Select(error => error.ToValidationFailure(propertyName));
        }

        [NotNull, ItemNotNull]
        public static IEnumerable<ValidationFailure> ToValidationFailures(
            [NotNull, ItemNotNull] this IEnumerable<IdentityError> errors,
            [NotNull] params (string Property, string Alias)[] propertyNames)
        {
            if (errors == null) throw new ArgumentNullException(nameof(errors));

            return errors
                        .GroupBy(e => propertyNames.FirstOrDefault(name => e.Description.IndexOf(name.Alias, StringComparison.OrdinalIgnoreCase) >= 0))
                        .SelectMany(group => group.ToValidationFailures(group.Key.Property ?? "request"));
        }
    }
}
