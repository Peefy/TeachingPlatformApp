using System.Runtime.InteropServices;

namespace TeachingPlatformApp.ZyPlatform
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ScoreDef
    {
        public short MsgType;

        public short Length;

        public short PlayerNum;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public short[] PlayerScore;
    }
}
