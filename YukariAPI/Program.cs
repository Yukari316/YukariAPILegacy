using System.Threading.Tasks;

namespace YukariAPI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ApiServer server = new ApiServer(19200, true);
            //启动服务器
            await server.StartServer();
        }
    }
}
