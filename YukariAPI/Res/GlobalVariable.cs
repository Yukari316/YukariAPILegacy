using System.Collections.Concurrent;

namespace YukariAPI.Res
{
    public static class GlobalVariable
    {
        /// <summary>
        /// 二次元怪话语录
        /// </summary>
        public static readonly ConcurrentDictionary<string,string[]> Nene = new();
    }
}