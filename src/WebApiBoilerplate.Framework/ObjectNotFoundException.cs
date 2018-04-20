using System;
using JetBrains.Annotations;

namespace WebApiBoilerplate.Framework
{
    public class ObjectNotFoundException : WebApiBoilerplateException
    {
        public ObjectNotFoundException([NotNull] Type type, long id)
            : base($"{type.Name} #{id} is not found")
        {
        }
    }
}
