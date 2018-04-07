using System;
using JetBrains.Annotations;

namespace WebApiBoilerplate.Framework
{
    public abstract class WebApiBoilerplateException : Exception
    {
        [NotNull]
        public string ExceptionId { get; } = Guid.NewGuid().ToString("n");

        [NotNull] 
        public virtual string Code => GetType().Name;

        protected WebApiBoilerplateException(string message) : base(message)
        { }

        protected WebApiBoilerplateException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
