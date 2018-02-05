using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Agile.Repository.Proxy;
using Agile.Repository.Utils;
using Autofac;

namespace Agile.Repository.Autofac
{
    public static class AutofacExt
    {
        public static void RegisterAgileRepository<T>(this ContainerBuilder builder) where T : class
        {
            var instance = AgileRepository.Proxy.SingletonInstance<T>();
            builder.RegisterInstance(instance).As<T>();
        }

        public static void RegisterAgileRepositories(this ContainerBuilder builder, string assembleyName)
        {
            var assembly = Assembly.Load(assembleyName);

            var types = assembly.GetTypes();
            var assignTypeFrom = typeof(IAgileRepository<>);
            foreach (var t in types)
            {
                if (!t.IsInterface)
                {
                    continue;
                }
                if (t.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == assignTypeFrom))
                {
                    var instance = GenericCallHelper.RunGenericMethod(AgileRepository.Proxy.GetType(),
                        "SingletonInstance", new Type[]{ t }, AgileRepository.Proxy,new object[]{});
                    builder.RegisterInstance(instance).As(t);
                }
            }
        }
    }
}
