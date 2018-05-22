using System.Runtime.InteropServices;

namespace TeachingPlatformApp.ZyPlatform
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GameStateFlag
    {
        public short Type;

        public short Length;

        public short Flag;
    }
}
