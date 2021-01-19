using System;
using SqlSugar;

namespace YukariAPI.Database
{
    public class SugarUtils
    {
        #region Client简单创建函数
        /// <summary>
        /// 创建一个SQLiteClient
        /// </summary>
        /// <returns>默认开启的SqlSugarClient</returns>
        internal static SqlSugarClient CreateSqlSugarClient()
        {
            string server = Environment.GetEnvironmentVariable("server");
            string user   = Environment.GetEnvironmentVariable("user");
            string passwd = Environment.GetEnvironmentVariable("passwd");

            return new (new ConnectionConfig
            {
                ConnectionString      = $"Server={server};Database=setu;Uid={user};Pwd={passwd};",
                DbType                = DbType.MySql,
                IsAutoCloseConnection = true,
                InitKeyType           = InitKeyType.Attribute
            });
        }
        #endregion
    }
}
