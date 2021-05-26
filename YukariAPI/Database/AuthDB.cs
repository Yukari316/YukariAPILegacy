using YukariAPI.Enumeration;

namespace YukariAPI.Database
{
    public static class AuthDB
    {
        /// <summary>
        /// 获取权限等级
        /// </summary>
        /// <param name="apikey">apikey</param>
        public static AuthLevel GetAuthLevel(string apikey)
        {
            using var client = SugarUtils.CreateSqlSugarClient();
            if (!client.Queryable<Auth>().Any(auth => auth.ApiKey == apikey)) return AuthLevel.None;
            return client.Queryable<Auth>()
                         .Where(auth => auth.ApiKey == apikey)
                         .Select(auth => auth.Level)
                         .First();
        }

        /// <summary>
        /// 获取apikey今日调用次数
        /// </summary>
        /// <param name="apikey">apikey</param>
        public static int GetApiKeyRequestCount(string apikey)
        {
            using var client = SugarUtils.CreateSqlSugarClient();
            return client.Queryable<Auth>()
                         .Where(auth => auth.ApiKey == apikey)
                         .Select(auth => auth.RequestCount)
                         .First();
        }

        /// <summary>
        /// 更新数据库计数
        /// </summary>
        /// <param name="apikey">apikey</param>
        /// <param name="newCount">新计数</param>
        public static bool ApiKeyRequestCountUpdate(string apikey, int newCount)
        {
            using var client = SugarUtils.CreateSqlSugarClient();
            return client.Updateable<Auth>()
                         .SetColumns(auth => auth.RequestCount == newCount)
                         .Where(auth => auth.ApiKey            == apikey)
                         .ExecuteCommandHasChange();
        }
    }
}