using System.Runtime.InteropServices;

namespace TeachingPlatformApp.ZyPlatform
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VectorAngExp
    {
        public float Y;

        public float X;

        public float Z;
    }

}
