using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YukariAPI.Enumeration;

namespace YukariAPI.Database
{
    public class AuthDB
    {
        /// <summary>
        /// 获取权限等级
        /// </summary>
        /// <param name="token">token</param>
        public static AuthLevel GetAuthLevel(string token)
        {
            using var client = SugarUtils.CreateSqlSugarClient();
            if (!client.Queryable<Auth>().Any(auth => auth.Token == token)) return AuthLevel.None;
            return client.Queryable<Auth>()
                         .Where(auth => auth.Token == token)
                         .Select(auth => auth.Level)
                         .First();
        }
    }
}
