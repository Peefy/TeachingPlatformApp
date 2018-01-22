using System.Runtime.InteropServices;

namespace TeachingPlatformApp.WswPlatform
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DataPacketToWsw
    {
        public byte Header { get; set; }
        public byte Index { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }

}
