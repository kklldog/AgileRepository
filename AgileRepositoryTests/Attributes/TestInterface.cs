using System;
using System.Collections.Generic;
using Agile.Repository.Attributes;
using Agile.Repository.Proxy;
using DapperExtensions.Mapper;

namespace AgileRepositoryTests.Attributes
{
    public class UserMapper : ClassMapper<User>
    {
        public UserMapper()
        {
            Table("Users");
            Map(x => x.Id).Key(KeyType.Assigned);
            AutoMap();
        }
    }
    public class User
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public DateTime CreateTime { get; set; }

    }
    public interface ITestInterface:IAgileRepository<User>
    {
        string Test(string p1);

        [QueryBySql("SELECT * FROM USERS")]
        IEnumerable<User> TestSql();

        [QueryBySql("SELECT * FROM USERS where username=@userName")]
        IEnumerable<User> TestSql1(string userName);

        [QueryByMethodName]
        IEnumerable<User> QueryByUserName(string userName);

        [QueryByMethodName]
        IEnumerable<User> QueryByUserNameAndId(string userName, string id);

        [QueryByMethodName]
        IEnumerable<User> QueryByCreaterIsNull();

        [QueryByMethodName]
        IEnumerable<User> QueryByCreaterIsNotNull();

        [CountBySql("Select count(*) from users")]
        int TestCount();

        [CountBySql("Select count(*) from users where userName=@userName")]
        int TestCount1(string userName);

        [CountByMethodName]
        int CountByUserName(string userName);

        [CountByMethodName]
        int CountByIdAndUserName(string id, string userName);

        [Insert]
        int Insert(User user);

        [Update]
        int Update(User user);

        [Delete]
        int Delete(User user);

        [DeleteByMethodName]
        int DeleteByUserName(string userName);

        [ExecuteBySql("Delete from [users] where id =@id ")]
        int Execute(string id);
    }
}
