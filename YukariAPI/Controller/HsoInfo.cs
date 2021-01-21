using System.Threading.Tasks;
using BeetleX.FastHttpApi;
using YukariAPI.Database;
using YukariAPI.Result;

namespace YukariAPI.Controller
{
    [Controller]
    public class HsoInfo
    {
        [Get(Route = "/setu/info_all_count")]
        public Task<CountSvgResult> GetPicCount()
            => Task.FromResult(new CountSvgResult("TOTAL", PicDB.GetPicCount(false), "000080", "BLUEVIOLET"));

        [Get(Route = "/setu/info_r18_count")]
        public Task<CountSvgResult> GetR18Count()
            => Task.FromResult(new CountSvgResult("R18", PicDB.GetPicCount(true), "000080", "ff69b4"));
    }
}
