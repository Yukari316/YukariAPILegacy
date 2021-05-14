using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PyLibSharp.Requests;
using YukariAPI.Database;
using YukariAPI.Tool;

namespace YukariAPI
{
    public class WebApis
    {
        /// <summary>
        /// P站API
        /// 用于获取作品信息
        /// </summary>
        /// <param name="pid">pid</param>
        public static async ValueTask<(bool success, string message, HsoPic picData)> GetPixivInfo(long pid)
        {
            try
            {
                var res = await Requests.GetAsync($"https://www.pixiv.net/ajax/illust/{pid}", new ReqParams
                {
                    Params = new Dictionary<string, string>
                    {
                        {"lang", "zh"}
                    },
                    Timeout = 5000
                });

                //检查返回数据
                var illustJson = res.Json();
                if (illustJson == null) return (false, "get null respose from pixiv", null);
                if (Convert.ToBoolean(illustJson["error"] ?? true))
                    return (false, $"pixivcat failed({illustJson["message"]})", null);
                
                //读取数据内容
                var pic = new HsoPic
                {
                    PicId = pid,
                    UserId = Convert.ToInt64(illustJson["body"]?["userId"] ?? -1),
                    Author = illustJson["body"]?["userName"]?.ToString() ?? string.Empty,
                    Title = illustJson["body"]?["title"]?.ToString() ?? string.Empty,
                    R18 = Convert.ToBoolean(illustJson["body"]?["xRestrict"] ?? true)
                };
                //处理tag
                var tagArray = illustJson["body"]?["tags"]?["tags"];
                if (tagArray == null) return (false, "can not get pic tags", null);
                List<string> tags = new();
                foreach (var tag in tagArray)
                {
                    //原tag
                    if(tag["tag"] != null)
                        tags.Add(tag["tag"].ToString());
                    //翻译后tag
                    if(tag["translation"]?["en"] != null)
                        tags.Add(tag["translation"]?["en"].ToString());
                }
                //为防止API中的xRestrict错误，使用tag再次判断R18值
                pic.R18 = tags.Any(tag => tag.Equals("R-18"));
                //写入tag
                pic.Tags = string.Join(",", tags);
                return (true, "OK", pic);
            }
            catch (Exception e)
            {
                ConsoleLog.Error("pixiv api error", ConsoleLog.ErrorLogBuilder(e));
                return (false, "", null);
            }
        }
        
        /// <summary>
        /// PixivCat代理连接生成
        /// </summary>
        /// <param name="pid">pid</param>
        public static async ValueTask<(bool success, string message, List<string> urls)>
            GetPixivCatInfo(long pid)
        {
            try
            {
                var res = await Requests.PostAsync("https://api.pixiv.cat/v1/generate", new ReqParams
                {
                    Header = new Dictionary<HttpRequestHeader, string>
                    {
                        {HttpRequestHeader.ContentType, "application/x-www-form-urlencoded; charset=UTF-8"}
                    },
                    PostContent =
                        new FormUrlEncodedContent(new[] {new KeyValuePair<string, string>("p", pid.ToString())}),
                    Timeout = 5000
                });
                if (res.StatusCode != HttpStatusCode.OK) return (false, $"pixivcat respose ({res.StatusCode})", null);
                //检查返回数据
                var proxyJson = res.Json();
                if (proxyJson == null) return (false, "get null respose from pixivcat", null);
                if (!Convert.ToBoolean(proxyJson["success"] ?? false))
                    return (false, $"pixivcat failed({proxyJson["error"]})", null);
                //是否为多张图片
                var urls = Convert.ToBoolean(proxyJson["multiple"] ?? false)
                    ? proxyJson["original_urls_proxy"]?.ToObject<List<string>>()
                    : new List<string> {proxyJson["original_url_proxy"]?.ToString() ?? string.Empty};
                return (true, "OK", urls);
            }
            catch (Exception e)
            {
                ConsoleLog.Error("pixiv api error", ConsoleLog.ErrorLogBuilder(e));
                return (false, $"pixiv api error ({e})", null);
            }
        }

        /// <summary>
        /// 一个计数器
        /// </summary>
        public static async ValueTask<byte[]> KawaiiCount()
        {
            try
            {
                var res = await Requests.GetAsync("https://count.getloli.com/get/@yukarisetuapi", new ReqParams
                {
                    Params = new Dictionary<string, string>
                    {
                        {"theme", "konachanrule34"}
                    },
                    Timeout = 5000
                });

                return res.StatusCode != HttpStatusCode.OK ? null : res.Content;
            }
            catch (Exception e)
            {
                ConsoleLog.Error("count api error", ConsoleLog.ErrorLogBuilder(e));
                return null;
            }
        }
    }
}
