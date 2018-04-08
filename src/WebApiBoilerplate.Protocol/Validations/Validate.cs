using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation;
using JetBrains.Annotations;

namespace WebApiBoilerplate.Protocol.Validations
{
    /// <summary>
    /// Validate protocol object
    /// </summary>
    public static class Validate
    {
        private static readonly ConcurrentDictionary<Assembly, Dictionary<Type, IValidator>> Validators =
            new ConcurrentDictionary<Assembly, Dictionary<Type, IValidator>>();

        /// <summary>
        /// Validate object and throw exeption on errors
        /// </summary>
        /// <typeparam name="TProtocol"></typeparam>
        /// <param name="obj"></param>
        public static void AndThrowOnError<TProtocol>([CanBeNull] TProtocol obj)
        {
            var validator = GetValidator<TProtocol>();

            if (obj != null)
            {
                validator.ValidateAndThrow(obj);
            }
        }

        [CanBeNull]
        private static IValidator<TProtocol> GetValidator<TProtocol>()
        {
            var type = typeof(TProtocol);

            var assemblyValidators = Validators.GetOrAdd(type.Assembly, assembly =>
            {
                var results = AssemblyScanner.FindValidatorsInAssembly(type.Assembly);

                var validators = results.Select(scanResult =>
                {
                    var validator = (IValidator)Activator.CreateInstance(scanResult.ValidatorType);
                    var protocolType = scanResult.InterfaceType.GetGenericArguments().First();
                    return new KeyValuePair<Type, IValidator>(protocolType, validator);
                }).ToDictionary(i => i.Key, i => i.Value);

                return validators;
            });

            if (assemblyValidators.TryGetValue(type, out var result))
            {
                return (IValidator<TProtocol>)result;
            }

            return null;
        }
    }
}
