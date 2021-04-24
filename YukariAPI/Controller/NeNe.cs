using System;
using System.Collections.Generic;
using System.Linq;
using BeetleX.FastHttpApi;
using JetBrains.Annotations;
using YukariAPI.Res;
using Utils = YukariAPI.Tool.Utils;

namespace YukariAPI.Controller
{
    [Controller]
    public class NeNe
    {
        /// <summary>
        /// 二次元语录API
        /// 呕
        /// </summary>
        /// <param name="keyword">查询关键词</param>
        [UsedImplicitly]
        [Get(Route = "/nene")]
        public JsonResult Nene(string keyword = null)
        {
            if (keyword == null)
            {
                var rd    = new Random();
                var key   = GlobalVariable.Nene.Keys.ToList()[rd.Next(0, GlobalVariable.Nene.Keys.Count - 1)];
                var words = GlobalVariable.Nene[key];
                return Utils.GenResult(new List<string> {words[rd.Next(0, words.Length - 1)]});
            }
            
            //查找怪话语录
            var searchRes = GlobalVariable.Nene
                                          .Where(i => i.Key.Contains(keyword))
                                          .Select(i => i.Value).ToList();
            if (searchRes.Count == 0) return Utils.GenResult(null, 404, "找不到你要的怪话嘛");
            //合并怪话语录
            var ret = new List<string>();
            foreach (var ne in searchRes)
            {
                ret.AddRange(ne);
            }

            return Utils.GenResult(ret);
        }
    }
}