using System.Collections.Generic;
using System.Linq;
using Dapper;
using DapperExtensions;

namespace Agile.Repository.Data
{
    internal class DapperRepository<T> : IRepository<T> where T : class
    {
        public virtual T Get(object id)
        {
            using (var conn = ConnectionFactory.CreateConnection())
            {
                return conn.Get<T>(id);
            }
        }

        public virtual IEnumerable<T> GetList()
        {
            using (var conn = ConnectionFactory.CreateConnection())
            {
                return conn.GetList<T>();
            }
        }

        public virtual bool Insert(T entity)
        {
            using (var conn = ConnectionFactory.CreateConnection())
            {
                conn.Insert(entity);
                return true;
            }
        }

        public virtual bool Update(T entity)
        {
            using (var conn = ConnectionFactory.CreateConnection())
            {
                return conn.Update(entity);
            }
        }

        public virtual bool Delete(T entity)
        {
            using (var conn = ConnectionFactory.CreateConnection())
            {
                return conn.Delete(entity);
            }
        }

        public IEnumerable<T> Query(string sql, object param)
        {
            using (var conn = ConnectionFactory.CreateConnection())
            {
                return conn.Query<T>(sql, param);
            }
        }

        public static IRepository<T> GetRepository()
        {
            return new DapperRepository<T>();
        }

    }
}
