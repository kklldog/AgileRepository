using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using AspectCore.DynamicProxy.Parameters;

namespace Agile.Repository.Attributes
{
    public abstract class SqlAttribute : AbstractInterceptorAttribute
    {
        public string ConnectionName { get; set; }
        public abstract override Task Invoke(AspectContext context, AspectDelegate next);


        protected Dictionary<string, object> ToParamterDict(ParameterCollection paramters)
        {
            var queryParams = new Dictionary<string, object>();
            foreach (var parameter in paramters)
            {
                queryParams.Add(parameter.Name, parameter.Value);
            }

            return queryParams;
        }
    }
}
