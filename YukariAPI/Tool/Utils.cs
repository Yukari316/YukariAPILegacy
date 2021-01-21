using System.Collections.Generic;
using BeetleX.FastHttpApi;
using YukariAPI.Enumeration;

namespace YukariAPI.Tool
{
    public static class Utils
    {
        /// <summary>
        /// API数据返回构造器
        /// </summary>
        /// <param name="data">数据数组</param>
        /// <param name="code">返回代码</param>
        /// <param name="message">API消息</param>
        public static JsonResult GenResult(List<object> data, int code = 0, string message = "OK")
        {
            return new(new
            {
                code,
                message,
                count = data?.Count ?? 0,
                data
            }, true);
        }

        /// <summary>
        /// API数据返回构造器
        /// </summary>
        /// <param name="data">数据数组</param>
        /// <param name="code">返回代码</param>
        /// <param name="message">API消息</param>
        public static JsonResult GenResult(object data, int code = 0, string message = "OK")
        {
            return new(new
            {
                code,
                message,
                data
            }, true);
        }

        /// <summary>
        /// 检查apikey使用限制
        /// </summary>
        /// <param name="level">权限等级</param>
        /// <param name="count">计数</param>
        public static bool ApiKeyLimitCheck(AuthLevel level, int count)
            => level switch
            {
                AuthLevel.None => count <= 10,
                AuthLevel.User => count <= 200,
                AuthLevel.Admin => true,
                _ => false
            };
    }
}
