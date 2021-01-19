using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PyLibSharp.Requests;
using YukariAPI.Tool;

namespace YukariAPI
{
    public class WebApis
    {
        public static async ValueTask<JToken> GetPixivInfo(long pid)
        {
            try
            {
                var res = await Requests.GetAsync("https://www.pixiv.net/ajax/illust/87168126?lang=zh", new ReqParams
                {
                    Timeout = 5000
                });

                return res.StatusCode != HttpStatusCode.OK ? null : res.Json();
            }
            catch (Exception e)
            {
                ConsoleLog.Error("pixiv api error", ConsoleLog.ErrorLogBuilder(e));
                return null;
            }
        }
    }
}
