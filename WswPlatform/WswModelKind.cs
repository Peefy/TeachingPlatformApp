namespace TeachingPlatformApp.WswPlatform
{
    /// <summary>
    /// Wsw模型类型
    /// </summary>
    public enum WswModelKind
    {
        /// <summary>
        /// 直升机
        /// </summary>
        Helicopter = 0,
        /// <summary>
        /// 战斗机
        /// </summary>
        Flighter = 1,
        /// <summary>
        /// 导弹
        /// </summary>
        Missile = 2,
        /// <summary>
        /// 第2个战斗机
        /// </summary>
        Flighter2 = 3,
        /// <summary>
        /// 所有模型
        /// </summary>
        All = Helicopter + Flighter + Flighter2 + Missile
    }
}
