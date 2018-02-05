using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AspectCore.DynamicProxy;

namespace Agile.Repository.Proxy
{
    public class InterfaceProxy : IInterfaceProxy
    {
        private readonly ConcurrentDictionary<Type, object> _proxyInsteances = new ConcurrentDictionary<Type, object>();
        protected IProxyGenerator ProxyGenerator = null;

        public InterfaceProxy()
        {
            var proxyGeneratorBuilder = new ProxyGeneratorBuilder();
            ProxyGenerator = proxyGeneratorBuilder.Build();
        }

        public T CreateProxyInstance<T>() where T : class
        {
            var proxyInstance = ProxyGenerator.CreateInterfaceProxy<T>();

            return proxyInstance;
        }

        public T SingletonInstance<T>() where T : class
        {
            object instance = null;
            var type = typeof(T);

            _proxyInsteances.TryGetValue(type, out instance);
            if (instance != null)
            {
                return instance as T;
            }

            T proxyInstance = CreateProxyInstance<T>();
            _proxyInsteances.TryAdd(type, proxyInstance);

            return proxyInstance;
        }

        public Type GetProxyType<T>() where T : class
        {
            return ProxyGenerator.TypeGenerator.CreateInterfaceProxyType(typeof(T));
        }
    }
}
