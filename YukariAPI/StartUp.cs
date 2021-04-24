using System.Threading.Tasks;
using YukariAPI.Tool;

namespace YukariAPI
{
    public static class StartUp
    {
        private static DataUpdater Updater { get; set; }

        private static async Task Main(string[] args)
        {
            //启动数据更新
            Updater = new DataUpdater();
            // dataUpdater.Update(null);
            //启动服务器
            var         server      = new ApiServer(19200);
            ConsoleLog.Info("Server","start server");
            //启动服务器
            await server.StartServer();
        }
    }
}
