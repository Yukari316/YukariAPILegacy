using SqlSugar;
using YukariAPI.Enumeration;

namespace YukariAPI.Database
{
    #region 表格定义
    /// <summary>
    /// 色图图库
    /// </summary>
    [SugarTable("setu")]
    public class HsoPic
    {
        [SugarColumn(ColumnName = "id", ColumnDataType = "bigint(11)", IsIdentity = true, IsPrimaryKey = true)]
        public long Id { get; set; }

        [SugarColumn(ColumnName = "pid", ColumnDataType = "bigint(11)")]
        public long PicId { get; set; }

        [SugarColumn(ColumnName = "index", ColumnDataType = "int(11)")]
        public long Index { get; set; }

        [SugarColumn(ColumnName = "uid", ColumnDataType = "bigint(11)")]
        public long UserId { get; set; }

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
        
        [SugarColumn(ColumnName = "uploader", ColumnDataType = "varchar(255)")]
        public string Uploader { get; set; }
    }

    /// <summary>
    /// 权限认证表
    /// </summary>
    [SugarTable("auth")]
    public class Auth
    {
        [SugarColumn(ColumnName = "apikey", ColumnDataType = "varchar(255)", IsPrimaryKey = true)]
        public string ApiKey { get; set; }

        [SugarColumn(ColumnName = "level", ColumnDataType = "int(11)")]
        public AuthLevel Level { get; set; }

        [SugarColumn(ColumnName = "requset_count", ColumnDataType = "int(11)")]
        public int RequestCount { get; set; }
        
        [SugarColumn(ColumnName = "desc", ColumnDataType = "varchar(64)")]
        public string Desc { get; set; }
    }
    #endregion
}
