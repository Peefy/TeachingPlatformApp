using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace TeachingPlatformApp.WswPlatform
{
    /// <summary>
    /// 战斗机，直升机初始坐标设置通信Packet
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PositionCommand
    {
        public int MsgType;
        public double X;
        public double Y;
        public double Z;
    }
}
