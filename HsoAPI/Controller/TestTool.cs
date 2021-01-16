using System;
using System.Threading.Tasks;
using BeetleX.FastHttpApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HsoAPI.Controller
{
    [Controller]
    public class TestTool
    {
        [Get(Route = "/")]
        public Task<object> Index(IHttpContext context)
        {
            return Task.FromResult<object>(new TextResult("Server Status\r\n"                                         +
                                                          $"Uptime     {DateTime.Now - context.Server.StartTime}\r\n" +
                                                          $"Requests   {context.Server.TotalRequest}\r\n"             +
                                                          $"Errors     {context.Server.RequestErrors}", true));
        }

        [Get]
        public Task<object> Test()
        {
            return Task.FromResult<object>(new JsonResult(new {Message = "好耶"}, true));
        }
    }
}
