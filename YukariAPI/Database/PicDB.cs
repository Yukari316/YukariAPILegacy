using System.Collections.Generic;

namespace YukariAPI.Database
{
    public class PicDB
    {
        /// <summary>
        /// 查找图片
        /// </summary>
        /// <param name="id">数据库id</param>
        public static Setu GetPicById(int id)
        {
            using var client = SugarUtils.CreateSqlSugarClient();
            return client.Queryable<Setu>()
                         .InSingle(id);
        }

        /// <summary>
        /// 查询数据库图片计数
        /// </summary>
        /// <param name="r18">r18开关</param>
        public static int GetPicCount(bool r18)
        {
            using var client = SugarUtils.CreateSqlSugarClient();
            return client.Queryable<Setu>()
                         .Where(pic => pic.R18 == r18)
                         .Count();
        }

        /// <summary>
        /// 获取图片id列表
        /// </summary>
        /// <param name="r18">r18开关</param>
        public static List<int> GetAllIdlList(bool r18)
        {
            using var client = SugarUtils.CreateSqlSugarClient();
            return client.Queryable<Setu>()
                         .Where(pic => pic.R18 == r18)
                         .Select(id => id.Id)
                         .ToList();
        }
    }
}
