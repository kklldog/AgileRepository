using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Repository.Attributes;
using Agile.Repository.Proxy;

namespace AgileRepositoryTests.Proxy
{
    public interface ITestInterface:IAgileRepository<Users>
    {
        string Test(string p1);

        [QueryBySql("SELECT * FROM USERS")]
        IEnumerable<Users> TestSql();

        [QueryBySql("SELECT * FROM USERS where username=@userName")]
        IEnumerable<Users> TestSql1(string userName);

        [QueryByName]
        IEnumerable<Users> QueryByUserName(string userName);

        [QueryByName]
        IEnumerable<Users> QueryByUserNameAndId(string userName, string id);

        [QueryByName]
        IEnumerable<Users> QueryByCreaterIsNull();

        [QueryByName]
        IEnumerable<Users> QueryByCreaterIsNotNull();

        [CountBySql("Select count(*) from users")]
        int TestCount();

        [CountBySql("Select count(*) from users where userName=@userName")]
        int TestCount1(string userName);

        [CountByName]
        int CountByUserName(string userName);

        [CountByName]
        int CountByIdAndUserName(string id, string userName);
    }
}
