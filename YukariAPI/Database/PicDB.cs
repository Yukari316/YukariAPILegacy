using System;
using System.Collections.Generic;
using System.Text;
using YukariToolBox.FormatLog;

namespace YukariAPI.Database
{
    public class PicDB
    {
        /// <summary>
        /// 查找图片
        /// </summary>
        /// <param name="id">数据库id</param>
        public static HsoPic GetPicById(long id)
        {
            try
            {
                using var client = SugarUtils.CreateSqlSugarClient();
                return client.Queryable<HsoPic>()
                             .InSingle(id);
            }
            catch (Exception e)
            {
                Log.Error("Database", Log.ErrorLogBuilder(e));
                return null;
            }
        }

        /// <summary>
        /// 查询数据库图片计数
        /// </summary>
        /// <param name="r18">r18开关</param>
        public static int GetPicCount(bool r18)
        {
            try
            {
                using var client = SugarUtils.CreateSqlSugarClient();
                return r18
                    ? client.Queryable<HsoPic>()
                            .Where(pic => pic.R18 == r18)
                            .Count()
                    : client.Queryable<HsoPic>()
                            .Count();
            }
            catch (Exception e)
            {
                Log.Error("Database", Log.ErrorLogBuilder(e));
                return -1;
            }
        }

        /// <summary>
        /// 根据pid查询图片数量
        /// </summary>
        /// <param name="pid">pid</param>
        public static int GetPicCountByPid(long pid)
        {
            try
            {
                using var client = SugarUtils.CreateSqlSugarClient();
                return client.Queryable<HsoPic>()
                             .Where(pic => pic.PicId == pid)
                             .Count();
            }
            catch (Exception e)
            {
                Log.Error("Database", Log.ErrorLogBuilder(e));
                return -1;
            }
        }

        /// <summary>
        /// 获取图片id列表
        /// </summary>
        /// <param name="r18">r18开关</param>
        public static List<long> GetAllIdlList(bool r18)
        {
            try
            {
                using var client = SugarUtils.CreateSqlSugarClient();
                return client.Queryable<HsoPic>()
                             .Where(pic => pic.R18 == r18)
                             .Select(id => id.Id)
                             .ToList();
            }
            catch (Exception e)
            {
                Log.Error("Database", $"{Log.ErrorLogBuilder(e)}");
                return null;
            }
        }

        /// <summary>
        /// 查找图片是否存在
        /// </summary>
        /// <param name="pid">pid</param>
        /// <param name="index">index</param>
        public static bool PicExitis(long pid, int index)
        {
            try
            {
                using var client = SugarUtils.CreateSqlSugarClient();
                return client.Queryable<HsoPic>()
                             .Any(pic => pic.PicId == pid && pic.Index == index);
            }
            catch (Exception e)
            {
                Log.Error("Database", Log.ErrorLogBuilder(e));
                return true;
            }
        }

        /// <summary>
        /// 查找图片是否存在
        /// </summary>
        /// <param name="pid">pid</param>
        public static bool PicExitis(long pid)
        {
            try
            {
                using var client = SugarUtils.CreateSqlSugarClient();
                return client.Queryable<HsoPic>()
                             .Any(pic => pic.PicId == pid);
            }
            catch (Exception e)
            {
                Log.Error("Database", Log.ErrorLogBuilder(e));
                return true;
            }
        }

        /// <summary>
        /// 插入新的图片信息
        /// </summary>
        /// <param name="pic">图片信息</param>
        public static int InsertNewPic(HsoPic pic)
        {
            try
            {
                using var client = SugarUtils.CreateSqlSugarClient();
                var       sql    = new StringBuilder();
                //ORM插入不支持四字节UTF8我傻了
                sql.Append(@"INSERT INTO `setu` (`pid`,`index`,`uid`,`title`,`author`,`r18`,`tags`,`url`,`uploader`) ");
                sql.Append($@"VALUES ('{pic.PicId}','{pic.Index}','{pic.UserId}',");
                sql.Append($@"FROM_BASE64('{Convert.ToBase64String(Encoding.UTF8.GetBytes(pic.Title))}'),");
                sql.Append($@"FROM_BASE64('{Convert.ToBase64String(Encoding.UTF8.GetBytes(pic.Author))}'),");
                sql.Append($@"'{Convert.ToInt32(pic.R18)}',");
                sql.Append($@"FROM_BASE64('{Convert.ToBase64String(Encoding.UTF8.GetBytes(pic.Tags))}'),");
                sql.Append($@"'{pic.Url}','{pic.Uploader}');");
                sql.Append("SELECT LAST_INSERT_ID();");

                return client.Ado.GetInt(sql.ToString());
            }
            catch (Exception e)
            {
                Log.Error("Database", Log.ErrorLogBuilder(e));
                return -1;
            }
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="pid">pid</param>
        public static bool DeletePic(long pid)
        {
            try
            {
                using var client = SugarUtils.CreateSqlSugarClient();
                return client.Deleteable<HsoPic>()
                             .Where(pic => pic.PicId == pid)
                             .ExecuteCommandHasChange();
            }
            catch (Exception e)
            {
                Log.Error("Database", Log.ErrorLogBuilder(e));
                return false;
            }
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="pid">pid</param>
        /// <param name="index">index</param>
        public static bool DeletePic(long pid, int index)
        {
            try
            {
                using var client = SugarUtils.CreateSqlSugarClient();
                return client.Deleteable<HsoPic>()
                             .Where(pic => pic.PicId == pid && pic.Index == index)
                             .ExecuteCommandHasChange();
            }
            catch (Exception e)
            {
                Log.Error("Database", Log.ErrorLogBuilder(e));
                return false;
            }
        }
    }
}