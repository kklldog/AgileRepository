using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Repository.Utils
{
    class GenericCallHelper
    {
        public static object RunGenericMethod(Type type, string method, Type[] genericTypeArguments, object instance, object[] paramters)
        {
            return GetGenericMethod(type, method, genericTypeArguments)
                 .Invoke(instance, paramters);
        }

        public static MethodInfo GetGenericMethod(Type type, string method, Type[] genericTypeArguments)
        {
            return type.GetMethod(method).MakeGenericMethod(genericTypeArguments);

        }
    }
}
