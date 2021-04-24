using System;
using System.Threading.Tasks;
using BeetleX.FastHttpApi;
using JetBrains.Annotations;

namespace YukariAPI.Controller
{
    [Controller]
    public class TestTool
    {
        [UsedImplicitly]
        [Get(Route = "/")]
        public Task<object> Index(IHttpContext context)
        {
            return Task.FromResult<object>(new TextResult("Server Status\r\n"                                         +
                                                          $"Uptime     {DateTime.Now - context.Server.StartTime}\r\n" +
                                                          $"Requests   {context.Server.TotalRequest}\r\n"             +
                                                          $"Errors     {context.Server.RequestErrors}", true));
        }
    }
}
