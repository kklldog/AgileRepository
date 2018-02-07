#region author info
// CreatedBy zhouminjie
// 2016-06-20
#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using DapperExtensions;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;

namespace Agile.Repository.Data
{
    internal class ConnectionFactory
    {
        public static IDbConnection CreateConnection<T>(string connectionName = "") where T : IDbConnection, new()
        {
            IDbConnection connection = new T();
            connection.ConnectionString = string.IsNullOrEmpty(connectionName)
                ? ConnectionConfig.GetConnectionString(ConnectionConfig.DefaultConnName)
                : ConnectionConfig.GetConnectionString(connectionName);
            connection.Open();
            return connection;
        }


        public static IDbConnection SqlConnection()
        {
            return CreateConnection<SqlConnection>();
        }

        public static IDbConnection CreateConnection(string connectionName = "", bool open = true)
        {
            var connSettings = ConnectionConfig.GetConnectionStringSettings(string.IsNullOrEmpty(connectionName) ? ConnectionConfig.DefaultConnName : connectionName);
            var providerName = string.IsNullOrEmpty(connSettings.ProviderName) ?
                DbProviders.Sqlserver : connSettings.ProviderName;
            var conn = DbProviderFactories.GetFactory(providerName).CreateConnection();
            conn.ConnectionString = connSettings.ConnectionString;

            if (open)
            {
                conn.Open();
            }

            return conn;
        }

        public static IDatabase CreateDatabase(string connectionName = "")
        {
            var conn = CreateConnection(connectionName);
            var dialect = GetSqlDialect(conn);
            if (dialect == null)
            {
                throw new Exception($"Can not find a dialect for connection , {conn.GetType().FullName} .");
            }

            var config = new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), dialect);
            var db =new Database(conn,new SqlGeneratorImpl(config));

            return db;
        }

        public static ISqlDialect GetSqlDialect(IDbConnection conn)
        {
            if (conn is SqlConnection)
            {
                return new SqlServerDialect();
            }

            //oracle wait

            //mysql wait

            return null;
        }

        public static ISqlDialect GetSqlDialect(string provider)
        {
            if (DbProviders.Sqlserver.Equals(provider,StringComparison.CurrentCultureIgnoreCase))
            {
                return new SqlServerDialect();
            }
            if (DbProviders.Oracle.Equals(provider, StringComparison.CurrentCultureIgnoreCase))
            {
                return new OracleDialect();
            }
            //mysql wait

            return null;
        }
    }
}