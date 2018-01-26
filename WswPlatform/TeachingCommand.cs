using System.Runtime.InteropServices;

namespace TeachingPlatformApp.WswPlatform
{
    /// <summary>
    /// 飞行实验通信Packet
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TeachingCommand
    {
        public byte HeaderOne;

        public byte HeaderTwo;

        public byte ExperimentIndex;

        public byte IsStart;

        public float ReservedSingle1;

        public float ReservedSingle2;

    }

}
