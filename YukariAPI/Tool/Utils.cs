using System.Collections.Generic;
using BeetleX.FastHttpApi;

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
    }
}
