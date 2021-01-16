using System.Threading.Tasks;

namespace HsoAPI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //启动服务器
            await FastApiServer.InitServer();
        }
    }
}
