using System;
using System.Net;
using System.Threading;
using Newtonsoft.Json.Linq;
using PyLibSharp.Requests;
using YukariAPI.Res;
using YukariToolBox.FormatLog;

namespace YukariAPI.Tool
{
    /// <summary>
    /// 定时动态数据更新/缓存任务
    /// </summary>
    public class DataUpdater
    {
        private Timer UpdateTimer { get; }

        public DataUpdater()
        {
            UpdateTimer = new Timer(Update, null, TimeSpan.Zero, TimeSpan.FromDays(1));
        }

        public async void Update(object obj)
        {
            Log.Info("Updater", "Update server data");
            try
            {
                var res =
                    await Requests.GetAsync("https://raw.githubusercontent.com/Kyomotoi/AnimeThesaurus/main/data.json",
                                            new ReqParams
                                            {
                                                Timeout = 5000
                                            });
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    //处理获取的数据
                    var neneJson = JObject.Parse(res.Text);
                    foreach (var (key, word) in neneJson)
                    {
                        var wordArray = word.ToObject<string[]>();
                        GlobalVariable.Nene.AddOrUpdate(key, wordArray, (_, _) => wordArray);
                    }
                }
                
                Log.Info("Updater", "Update success");
            }
            catch (Exception e)
            {
                Log.Error("Date update error", ConsoleLog.ErrorLogBuilder(e));
            }
        }
    }
}