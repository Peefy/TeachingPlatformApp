using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace TeachingPlatformApp.WswPlatform
{
    /// <summary>
    /// Wsw姿态数据通信Packet
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AngleWithLocation
    {
        public double X;

        public double Y;

        public double Z;

        public double Yaw;

        public double Pitch;

        public double Roll;
    }
}
