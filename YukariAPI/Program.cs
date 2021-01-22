using System.Threading.Tasks;
using YukariAPI.Tool;

namespace YukariAPI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ApiServer server = new ApiServer(19200);
            ConsoleLog.Info("Server","start server");
            //启动服务器
            await server.StartServer();
        }
    }
}
