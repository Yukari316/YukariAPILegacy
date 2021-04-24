using System.Reflection;
using System.Threading.Tasks;
using BeetleX.EventArgs;
using BeetleX.FastHttpApi;
using YukariAPI.Enumeration;
using YukariAPI.Tool;

namespace YukariAPI
{
    public class ApiServer
    {
        #region 属性
        //属性
        internal HttpApiServer Server { get; set; }
        #endregion

        #region 构造方法

        /// <summary>
        /// 初始化服务器
        /// </summary>
        /// <param name="port">端口</param>
        /// <param name="useDebugLog">使用debug日志</param>
        public ApiServer(int port, bool useDebugLog = false)
        {
            Server              = new HttpApiServer();
            Server.Options.Host = "127.0.0.1";
            Server.Options.Port = port;
            //设置log
            Server.Options.LogLevel     = useDebugLog ? LogType.Debug : LogType.Info;
            Server.Options.LogToConsole = true;
            Server.Options.Debug        = useDebugLog;
            ConsoleLog.SetLogLevel(useDebugLog ? LogLevel.Debug : LogLevel.Info);
            Server.Options.CrossDomain = new OptionsAttribute
            {
                AllowOrigin = "*"
            };
            //注册所有控制器
            Server.Register(Assembly.GetExecutingAssembly());
        }
        #endregion

        #region 启动服务器
        public async ValueTask StartServer()
        {
            Server.Open();
            await Task.Delay(-1);
        }
        #endregion
    }
}
