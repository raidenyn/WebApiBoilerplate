using System;

namespace WebApiBoilerplate.Framework
{
    public class SystemException : WebApiBoilerplateException
    {
        public SystemException(string message) : base(message)
        {
        }

        public SystemException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
