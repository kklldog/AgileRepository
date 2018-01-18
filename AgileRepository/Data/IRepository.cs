#region author info
// CreatedBy zhouminjie
// 2016-06-20
#endregion

using System.Collections.Generic;
using DapperExtensions;

namespace Agile.Repository.Data
{
    internal interface IRepository
    {
        
    }

    internal interface IRepository<T>: IRepository where T:class
     {
        T Get(object id);

        IEnumerable<T> GetList();

        bool Insert(T entity);

        bool Update(T entity);

        bool Delete(T entity);

         IEnumerable<T> Query(string sql, object param);


     }
}