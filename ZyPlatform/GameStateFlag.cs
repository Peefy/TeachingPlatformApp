using System.Runtime.InteropServices;

namespace TeachingPlatformApp.ZyPlatform
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GameStateFlag
    {
        public short m_Type;

        public short m_length;

        public short m_Flag;
    }

}
