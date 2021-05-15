namespace YukariAPI.Enumeration
{
    /// <summary>
    /// 图库动作类型
    /// </summary>
    public enum HsoAction
    {
        /// <summary>
        /// 没有执行权限
        /// </summary>
        None,

        /// <summary>
        /// 随机色图
        /// </summary>
        RandomPic = 1,

        /// <summary>
        /// 添加色图
        /// </summary>
        AddPic = 2,

        /// <summary>
        /// 删除色图
        /// </summary>
        DeletePic = 3
    }
}