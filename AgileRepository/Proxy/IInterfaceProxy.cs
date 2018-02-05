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
        T CreateProxyInstance<T>() where T : class;

        T SingletonInstance<T>() where T : class;

        Type GetProxyType<T>() where T : class;

    }
}
