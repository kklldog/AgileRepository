# AgileRepository
这是一个可以帮助你快速开发Repository的lib。有点像Springdata JPA根据方法名、注解来自动生成查询方法的功能。  
对于一些简单的查询，只需要定义接口就行了，实现都不用。

    public interface IUserRepository:IAgileRepository<Users>
    {
        [QueryByMethodName]
        IEnumerable<Users> QueryByUserName(string userName);

    }
## 依赖
AspectCore  
Dapper  
DapperExtensions  
## 实例
### 根据sql查询 
        [QueryBySql("SELECT * FROM USERS")]
        IEnumerable<User> TestSql();

        [QueryBySql("SELECT * FROM USERS where username=@userName")]
        IEnumerable<User> TestSql1(string userName); 
### 根据方法名称查询
        [QueryByMethodName]
        IEnumerable<User> QueryByUserName(string userName);

        [QueryByMethodName]
        IEnumerable<User> QueryByUserNameAndId(string userName, string id);

        [QueryByMethodName]
        IEnumerable<User> QueryByCreaterIsNull();

        [QueryByMethodName]
        IEnumerable<User> QueryByCreaterIsNotNull(); 
### 查询所有
        [QueryAll]
        IEnumerable<User> QueryAll(); 
### 根据 sql Count
        [CountBySql("Select count(*) from users")]
        int TestCount();

        [CountBySql("Select count(*) from users where userName=@userName")]
        int TestCount1(string userName); 
### 根据方法名Count
        [CountByMethodName]
        int CountByUserName(string userName);

        [CountByMethodName]
        int CountByIdAndUserName(string id, string userName);
### Count所有
        [CountAll]
        int CountAll();
### Insert
        [Insert]
        int Insert(User user);
        [Insert]
        int Insert(IEnumerable<User> users);
### Update
        [Update]
        int Update(User user);

        [Update]
        int Update(IEnumerable<User> users);
### 
        [Delete]
        int Delete(User user);

        [Delete]
        int Delete(IEnumerable<User> users);

        [DeleteByMethodName]
        int DeleteByUserName(string userName);
### 执行非查询sql
        [ExecuteBySql("Delete from [users] where id =@id ")]
        int Execute(string id);
## 支持的where关键字
And QueryByUserNameAndId  
Or QueryByUserNameOrId  
IsNull QueryByUserNameIsNull  
IsNotNull QueryByUserNameIsNotNull  
