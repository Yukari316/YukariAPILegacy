using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeetleX.FastHttpApi;
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
        /// <param name="r18">r18开关</param>
        [Get(Route = "/setu")]
        public Task<object> RandomPic(bool r18 = false)
        {
            //获取ID列表
            var qPicList = PicDB.GetAllIdlList(r18);
            if (qPicList == null || qPicList.Count == 0)
            {
                return Task.FromResult<object>(Utils.GenResult(null, 100, "Database:Get Pic Index Failed"));
            }
            //随机选取ID
            Random rd       = new Random();
            var    randomId = rd.Next(0, qPicList.Count - 1);
            //获取图片信息
            var pic = PicDB.GetPicById(qPicList[randomId]);
            if (pic == null)
            {
                return Task.FromResult<object>(Utils.GenResult(null, 101, "Database:Get Pic Info Failed"));
            }

            return Task.FromResult<object>(Utils.GenResult(new List<object>
            {
                new
                {
                    pid    = pic.PicId,
                    uid    = pic.UserId,
                    title  = pic.Title,
                    author = pic.Author,
                    r18    = pic.R18,
                    tags   = pic.Tags.Split(','),
                    url    = pic.Url
                }
            }));
        }

        [Get(Route = "add_pic")]
        public Task<object> AddPic(string token = null, long pid = 0, int index = -1)
        {
            if (string.IsNullOrEmpty(token))
                return Task.FromResult<object>(Utils.GenResult(null, 403, "Auth:null token"));
            if (pid <= 0) return Task.FromResult<object>(Utils.GenResult(null, 400, "Auth:illegal pid"));

            //token检查
            AuthLevel level = AuthDB.GetAuthLevel(token);
            return Task.FromResult<object>(new JsonResult(new {l = level}));
        }
    }
}