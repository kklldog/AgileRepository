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
        private readonly List<Type> _agileRepositories = new List<Type>();
        private readonly ConcurrentDictionary<Type, IAgileRepository> _proxyInsteances = new ConcurrentDictionary<Type, IAgileRepository>();

        public void Init(AgileRepositoryConfig config)
        {
            _agileRepositories.Clear();
            _proxyInsteances.Clear();

            var assemblies = config == null
                ? AppDomain.CurrentDomain.GetAssemblies()
                : new Assembly[] { Assembly.Load(config.AssbemlyName) };

            foreach (var ass in assemblies)
            {
                if (ass.FullName.StartsWith("AspectCore.Abstractions"))
                {
                    continue;
                }
                var types = ass.GetTypes();
                var assignTypeFrom = typeof(IAgileRepository);
                foreach (var t in types)
                {
                    if (assignTypeFrom.IsAssignableFrom(t))
                    {
                        if (t.IsInterface)
                        {
                            _agileRepositories.Add(t);
                        }
                    }
                }
            }
        }

        public TRt Invoke<T, TRt>(string method, params object[] args) where T : class, IAgileRepository
        {
            var type = _agileRepositories.FirstOrDefault(t => t == typeof(T));
            var met = type.GetMethod(method);
            if (met == null)
            {
                throw new Exception($"Can not find Method:{method} in T:{type}");
            }
            var interfaceProxy = GetInstance<T>();
            var result = met.Invoke(interfaceProxy, args);

            return (TRt)result;
        }

        public T CreateProxyInstance<T>() where T : class, IAgileRepository
        {
            var proxyGeneratorBuilder = new ProxyGeneratorBuilder();
            IProxyGenerator proxyGenerator = proxyGeneratorBuilder.Build();
            var proxyInstance = proxyGenerator.CreateInterfaceProxy<T>();

            return proxyInstance;
        }

        public T GetInstance<T>() where T : class, IAgileRepository
        {
            var type = _agileRepositories.FirstOrDefault(t => t == typeof(T));
            if (type == null)
            {
                throw new Exception($"Can not find T:{typeof(T).FullName}");
            }

            IAgileRepository instance = null;
            _proxyInsteances.TryGetValue(type, out instance);
            if (instance != null)
            {
                return instance as T;
            }

            T proxyInstance = CreateProxyInstance<T>();

            _proxyInsteances.TryAdd(type, proxyInstance);

            return proxyInstance;
        }
    }
}
