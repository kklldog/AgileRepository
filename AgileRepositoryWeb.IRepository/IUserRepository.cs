using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Repository;
using Agile.Repository.Proxy;

namespace AgileRepositoryWeb.IRepository
{
    public class Users
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public interface IUserRepository : IAgileRepository
    {
        [AutoSql(Sql = "select * from [users] where userName = @userName")]
        IEnumerable<Users> QueryBySql(string userName);
    }
}
