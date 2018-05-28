using System.Runtime.InteropServices;

namespace TeachingPlatformApp.WswPlatform
{
    /// <summary>
    /// 航路点设置通信Packet
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct WayPointCommand
    {
        /// <summary>
        /// 通信帧头
        /// </summary>
        public double Header { get; set; }
        /// <summary>
        /// 航路点索引
        /// </summary>
        public double Index { get; set; }
        /// <summary>
        /// 航路点X坐标
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// 航路点Y坐标
        /// </summary>
        public double Y { get; set; }
    }

}
