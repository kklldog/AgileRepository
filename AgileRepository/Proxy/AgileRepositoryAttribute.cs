using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Repository.Proxy
{
    public class AgileRepositoryAttribute : Attribute
    {

    }

    public interface IAgileRepository<TEntity> where TEntity:class 
    {
        
    }
}
