using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeetleX.FastHttpApi;
using YukariAPI.Database;

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
        public Task<object> RandomPic(bool r18 = false, string tag = null)
        {
            using var client = SugarUtils.CreateSqlSugarClient();
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
    }
}