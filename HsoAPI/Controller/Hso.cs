using System;
using System.Threading.Tasks;
using BeetleX.FastHttpApi;
using HsoAPI.Database;

namespace HsoAPI.Controller
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
            using var client = SugarUtils.CreateSqlSugarClient();
            //获取ID列表
            var qPicList = PicDB.GetAllIdlList(r18);
            if (qPicList == null || qPicList.Count == 0)
            {
                return Task.FromResult<object>(new JsonResult(new
                {
                    code = 1 
                }));
            }
            //随机选取ID
            Random rd       = new Random();
            var    randomId = rd.Next(0, qPicList.Count - 1);
            //获取图片信息
            var pic = PicDB.GetPicById(qPicList[randomId]);
            var picRes = new
            {
                pid    = pic.PicId,
                uid    = pic.UserId,
                title  = pic.Title,
                author = pic.Author,
                r18    = pic.R18,
                tags   = pic.Tags.Split(','),
                url    = pic.Url
            };

            return Task.FromResult<object>(new JsonResult(new
            {
                code    = 0,
                message = "success",
                count   = 1,
                data    = picRes
            }));
        }
    }
}
