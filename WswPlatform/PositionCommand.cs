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
        public int MessageType;
        public int MessageSender;

        public double Timestamp;

        public Glmdvec3 Pos;
        public Glmdvec3 Dir;
        public Glmdvec3 Up;

        public Glmdvec3 Velocity;
        public Glmdvec3 AngularVelocity;

        public Glmdvec3 Acceleration;
        public Glmdvec3 AngularAccleration;

        public Glmdvec3 EyePos;
        public double EngineRpm;
        public double Lon;
        public double Lat;

        public double[,] Engines;

        public int Locked;

        public Glmdvec3[] Position;

        public double[] Yaw;
        public double[] Pitch;
        public double[] Roll;

        public int Hit;
        public int[] Fire;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Glmdvec3
    {
        public double X;
        public double Y;
        public double Z;
    }

}
