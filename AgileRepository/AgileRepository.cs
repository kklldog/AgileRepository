using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Repository.Proxy;

namespace Agile.Repository
{
    public static class AgileRepository
    {
        private static AgileRepositoryConfig _config;
        private static IInterfaceProxy _proxy;
        private static readonly object _lock = new object();
        public static IInterfaceProxy Proxy
        {
            get
            {
                if (_proxy == null)
                {
                    lock (_lock)
                    {
                        if (_proxy == null)
                        {
                            _proxy = new InterfaceProxy();
                        }
                    }
                }

                return _proxy;
            }
        }

        public static void Config(AgileRepositoryConfig config)
        {
            _config = config;
        }

        public static void Init(AgileRepositoryConfig config)
        {
            _config = config;
            Proxy.Init(_config);
        }
    }
}
