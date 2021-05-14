namespace YukariAPI.Enumeration
{
    public enum AuthLevel
    {
        /// <summary>
        /// 无权限
        /// </summary>
        None = 0,
        /// <summary>
        /// 仅获取图片
        /// </summary>
        User = 1,
        /// <summary>
        /// 图库管理员
        /// </summary>
        Admin = 2
    }
}
