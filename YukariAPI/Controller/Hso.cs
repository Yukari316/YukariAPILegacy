using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeetleX.FastHttpApi;
using JetBrains.Annotations;
using YukariAPI.Database;
using YukariAPI.Enumeration;
using Utils = YukariAPI.Tool.Utils;

namespace YukariAPI.Controller
{
    [Controller]
    public class Hso
    {
        /// <summary>
        /// 随机色图
        /// </summary>
        /// <param name="context">请求信息</param>
        /// <param name="r18">r18开关</param>
        [UsedImplicitly]
        [Get(Route = "/setu/")]
        public JsonResult RandomPic(IHttpContext context, bool r18 = false)
        {
            //获取ID列表
            var qPicList = PicDB.GetAllIdlList(r18);
            if (qPicList == null || qPicList.Count == 0)
            {
                return Utils.GenResult(null, 500, "Database:Get Pic Index Failed");
            }
            //随机选取ID
            Random rd       = new Random();
            var    randomId = rd.Next(0, qPicList.Count - 1);
            //获取图片信息
            var pic = PicDB.GetPicById(qPicList[randomId]);
            if (pic == null)
            {
                return Utils.GenResult(null, 500, "Database:Get Pic Index Failed");
            }
            //计数器
            Task.Run(async () =>
                     {
                         await WebApis.KawaiiCount();
                     });

            return Utils.GenResult(new List<object>
            {
                new
                {
                    pid    = pic.PicId,
                    index  = pic.Index,
                    uid    = pic.UserId,
                    title  = pic.Title,
                    author = pic.Author,
                    r18    = pic.R18,
                    tags   = pic.Tags.Split(','),
                    url    = pic.Url
                }
            });
        }

        /// <summary>
        /// 色图添加功能
        /// </summary>
        /// <param name="apikey">key</param>
        /// <param name="action">动作期望</param>
        [UsedImplicitly]
        [Get(Route = "/setu/auth")]
        public JsonResult ApiKeyVerify(string apikey = null, int action = -1)
        {
            if (string.IsNullOrEmpty(apikey))
                return Utils.GenResult(null, 400, "Auth:Request refused(illegal apikey)");
            //apikey检查
            AuthLevel level = AuthDB.GetAuthLevel(apikey);
            return Utils.GenResult(new
            {
                Level = level,
                Actions = level switch
                {
                    AuthLevel.Admin => new[] {HsoAction.RandomPic, HsoAction.AddPic, HsoAction.DeletePic},
                    AuthLevel.None => new[] {HsoAction.None},
                    AuthLevel.User => new[] {HsoAction.RandomPic},
                    _ => new[] {HsoAction.None}
                }
            });
        }

        /// <summary>
        /// 色图添加功能
        /// </summary>
        /// <param name="apikey">apikey</param>
        /// <param name="pid">pid</param>
        /// <param name="index">index</param>
        [UsedImplicitly]
        [Get(Route = "/setu/add_pic")]
        public async Task<JsonResult> AddPic(string apikey = null, long pid = 0, int index = -1)
        {
            if (string.IsNullOrEmpty(apikey))
                return Utils.GenResult(null, 403, "Auth:Request refused(illegal apikey)");
            //apikey检查
            var authLevel = AuthDB.GetAuthLevel(apikey);
            switch (authLevel)
            {
                case AuthLevel.None:case AuthLevel.User:
                    return Utils.GenResult(null, 403, "Auth:Request refused(access denied)");
                case AuthLevel.Admin:
                    break;
                default:
                    return Utils.GenResult(null, 400, "Auth:Request refused(unknown apikey)");
            }
            //检查pid
            if (pid <= 0) return Utils.GenResult(null, 400, "Argument:Argument out of range(pid)");

            //更新数据库计数
            if (!AuthDB.ApiKeyRequestCountUpdate(apikey, AuthDB.GetApiKeyRequestCount(apikey) + 1))
            {
                return Utils.GenResult(null, 500, "Database:Database error(update apikey count)");
            }

            //获取图片代理链接
            var proxyUrls = await WebApis.GetPixivCatInfo(pid);
            if (!proxyUrls.success)
                return Utils.GenResult(null, -1,
                                       $"WebAPI:PixivCat api error({proxyUrls.message})");
            //判断图片是否已经存在
            int picCount  = PicDB.GetPicCountByPid(pid);
            if (picCount == -1)
                return Utils.GenResult(null, 500,
                                       "Database:Database error(get pic count failed)");
            if (picCount >= proxyUrls.urls.Count)
                return Utils.GenResult(null, -2, "Pic:Pic existed");
            if (index > proxyUrls.urls.Count - 1) return Utils.GenResult(null, -3, "Pic:illegal Pic index");
            //查找图片信息
            var picApiRes = await WebApis.GetPixivInfo(pid);
            if (!picApiRes.success)
                return Utils.GenResult(null, -1,
                                       $"WebAPI:Pixiv api error({picApiRes.message})");
            var picInfo      = picApiRes.picData;
            //写入上传者
            picInfo.Uploader = apikey;
            var successCount = 0;

            //写入数据库
            for (var i = 0; i < proxyUrls.urls.Count; i++)
            {
                if (PicDB.PicExitis(pid, i)) continue;
                //写入对应图片信息
                picInfo.Index = i;
                picInfo.Url   = proxyUrls.urls[i];
                var id = PicDB.InsertNewPic(picInfo);
                if (id == -1)
                    return Utils.GenResult(null, 500,
                                          "Database:Database error(add new pic failed)");
                successCount++;
            }
            //返回数据
            return Utils.GenResult(new
            {
                picData = new
                {
                    picInfo.PicId,
                    picInfo.UserId,
                    picInfo.Title,
                    picInfo.Author,
                    picInfo.R18,
                    picInfo.Tags
                },
                successCount,
                proxyUrls.urls
            });
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="apikey">apikey</param>
        /// <param name="pid">pid</param>
        /// <param name="index">index</param>
        [UsedImplicitly]
        [Get(Route = "setu/del_pic")]
        public Task<JsonResult> DelPic(string apikey = null, long pid = 0, int index = -1)
        {
            if (string.IsNullOrEmpty(apikey))
                return Task.FromResult(Utils.GenResult(null, 403, "Auth:Request refused(illegal apikey)"));
            //apikey检查
            AuthLevel level = AuthDB.GetAuthLevel(apikey);
            switch (level)
            {
                case AuthLevel.None:case AuthLevel.User:
                    return Task.FromResult(Utils.GenResult(null, 403, "Auth:Request refused(access denied)"));
                case AuthLevel.Admin:
                    break;
                default:
                    return Task.FromResult(Utils.GenResult(null, 400, "Auth:Request refused(unknown apikey)"));
            }
            //检查pid
            if (pid <= 0) return Task.FromResult(Utils.GenResult(null, 400, "Argument:Argument out of range(pid)"));

            //更新数据库计数
            if (!AuthDB.ApiKeyRequestCountUpdate(apikey, AuthDB.GetApiKeyRequestCount(apikey) + 1))
            {
                return Task.FromResult(Utils.GenResult(null, 500, "Database:Database error(update apikey count)"));
            }

            //查找图片是否存在
            if(!PicDB.PicExitis(pid)) return Task.FromResult(Utils.GenResult(null, 404, "Database:Pic not found"));
            //删除图片
            if (index == -1)
            {
                if(!PicDB.DeletePic(pid)) return Task.FromResult(Utils.GenResult(null, 500, "Database:Pic delete failed"));
            }
            else
            {
                if(!PicDB.DeletePic(pid, index)) return Task.FromResult(Utils.GenResult(null, 500, "Database:Pic delete failed"));
            }
            return Task.FromResult(Utils.GenResult(new {}));
        }
    }
}