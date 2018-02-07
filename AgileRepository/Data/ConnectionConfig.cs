#region author info
// CreatedBy zhouminjie
// 2016-06-20
#endregion

using System;
using System.Configuration;

namespace Agile.Repository.Data
{
    public class ConnectionConfig
    {
        public const string DefaultConnName = "defaultConn";

        public static string GetConnectionString(string connectionName)
        {
            return GetConnectionStringSettings(connectionName).ConnectionString;
        }

        public static string GetProviderName(string connectionName)
        {
            return GetConnectionStringSettings(connectionName).ProviderName;
        }

        public static ConnectionStringSettings GetConnectionStringSettings(string connectionName)
        {
            if (string.IsNullOrEmpty(connectionName))
            {
                throw new ArgumentNullException(connectionName);
            }
            var conn = ConfigurationManager.ConnectionStrings[connectionName];
            if (conn == null)
            {
                throw new ArgumentNullException(connectionName,
                    string.Format("Can not find GetConnectionStringSettings by name:{0}", connectionName));
            }

            return conn;
        }
    }


}

