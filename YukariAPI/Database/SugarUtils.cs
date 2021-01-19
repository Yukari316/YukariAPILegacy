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

    #region 表格定义
    [SugarTable("setu")]
    public class Setu
    {
        [SugarColumn(ColumnName = "id", ColumnDataType = "int(11)", IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }

        [SugarColumn(ColumnName = "pid", ColumnDataType = "int(11)")]
        public int PicId { get; set; }

        [SugarColumn(ColumnName = "uid", ColumnDataType = "int(11)")]
        public int UserId { get; set; }

        [SugarColumn(ColumnName = "title", ColumnDataType = "varchar(255)")]
        public string Title { get; set; }

        [SugarColumn(ColumnName = "author", ColumnDataType = "varchar(255)")]
        public string Author { get; set; }

        [SugarColumn(ColumnName = "r18", ColumnDataType = "int(1)")]
        public bool R18 { get; set; }

        [SugarColumn(ColumnName = "tags", ColumnDataType = "varchar(1024)")]
        public string Tags { get; set; }

        [SugarColumn(ColumnName = "url", ColumnDataType = "varchar(255)")]
        public string Url { get; set; }
    }

    #endregion
}
