using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using BeetleX.Buffers;
using BeetleX.FastHttpApi;
using PyLibSharp.Requests;
using YukariAPI.Res;
using YukariToolBox.FormatLog;

namespace YukariAPI.Result
{
    public class CountSvgResult : ResultBase
    {
        /// <summary>
        /// XML文本内容
        /// </summary>
        private string XmlString { get; set; }

        /// <summary>
        /// 构造SVG返回类型
        /// </summary>
        /// <param name="name">label名</param>
        /// <param name="count">计数值</param>
        /// <param name="labelColorStr">颜色</param>
        /// <param name="colorStr">颜色</param>
        public CountSvgResult(string name, int count, string labelColorStr, string colorStr)
        {
            try
            {
                var res = Requests.Get("https://img.shields.io/static/v1", new ReqParams
                {
                    Params = new Dictionary<string, string>
                    {
                        {"label", name},
                        {"labelColor", labelColorStr},
                        {"message", count.ToString()},
                        {"color", colorStr},
                        {"style", "flat-square"}
                    },
                    Timeout = 5000
                });
                //检查响应
                if (res.StatusCode != HttpStatusCode.OK)
                {
                    XmlString = Encoding.UTF8.GetString(Resource.Error);
                    return;
                }

                //写入XML
                XmlString = res.Text;
            }
            catch (Exception e)
            {
                Log.Error("svg get error", Log.ErrorLogBuilder(e));
                XmlString = Encoding.UTF8.GetString(Resource.Error);
            }
        }

        public override bool HasBody => true;

        //设置SVG内容类型
        public override IHeaderItem ContentType => new HeaderItem("Content-Type: image/svg+xml;charset=utf-8\r\n");

        //写入SVG XML
        public override void Write(PipeStream stream, HttpResponse response)
        {
            stream.Write(XmlString);
        }
    }
}