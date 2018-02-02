using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Repository.Proxy
{
    public interface IInterfaceProxy
    {
        T CreateProxyInstance<T>() where T : class, IAgileRepository;

        void Init(string[] assembliyNames);

        T GetInstance<T>() where T : class, IAgileRepository;

        TRt Invoke<T, TRt>(string method, params object[] args) where T : class, IAgileRepository;
    }
}
