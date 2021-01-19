namespace YukariAPI.Enumeration
{
    public enum AuthLevel
    {
        /// <summary>
        /// 无权限
        /// </summary>
        None,
        /// <summary>
        /// 仅获取图片
        /// </summary>
        User,
        /// <summary>
        /// 上传/获取图片
        /// </summary>
        UpLoadOnly,
        /// <summary>
        /// 图库管理员
        /// </summary>
        Admin
    }
}
