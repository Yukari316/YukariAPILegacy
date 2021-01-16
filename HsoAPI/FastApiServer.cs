using System.Threading.Tasks;
using BeetleX.EventArgs;
using BeetleX.FastHttpApi;
using HsoAPI.Controller;

namespace HsoAPI
{
    public class FastApiServer
    {
        internal static HttpApiServer Server;
        /// <summary>
        /// 启动服务器
        /// </summary>
        public static async Task InitServer()
        {
            Server                      = new HttpApiServer();
            Server.Options.Host         = "127.0.0.1";
            Server.Options.Port         = 19200;
            //关闭原log
            Server.Options.LogLevel     = LogType.Off;
            Server.Options.LogToConsole = false;
            Server.Options.Debug        = false;
            Server.Options.CrossDomain = new OptionsAttribute
            {
                AllowOrigin = "*"
            };
            //注册控制器
            Server.Register(typeof(TestTool).Assembly);
            //启动服务器
            Server.Open();
            await Task.Delay(-1);
        }
    }
}
