using System;
using MySql.Data.MySqlClient;
using SqlSugar;

namespace YukariAPI.Database
{
    public class SugarUtils
    {
        #region Client简单创建函数
        /// <summary>
        /// 创建一个SQLiteClient
        /// </summary>
        /// <returns>SqlSugarClient</returns>
        internal static SqlSugarClient CreateSqlSugarClient()
        {
            var connectionStrBuilder =
                new MySqlConnectionStringBuilder
                {
                    Server       = Environment.GetEnvironmentVariable("server"),
                    Port         = Convert.ToUInt32(Environment.GetEnvironmentVariable("port")),
                    Database     = "setu",
                    UserID       = Environment.GetEnvironmentVariable("user"),
                    Password     = Environment.GetEnvironmentVariable("passwd"),
                    CharacterSet = "utf8mb4"
                };
            return new SqlSugarClient(new ConnectionConfig
            {
                ConnectionString      = connectionStrBuilder.ToString(),
                DbType                = DbType.MySql,
                IsAutoCloseConnection = true,
                InitKeyType           = InitKeyType.Attribute
            });
        }
        #endregion
    }
}
