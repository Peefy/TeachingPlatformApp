using System;
using System.Runtime.InteropServices;

namespace TeachingPlatformApp.ZyPlatform
{
    /// <summary>
    /// 驱动720度平台运动旋转的数据格式
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AngleDef
    {
        public short MsgType;

        public short Length;

        public float Time;

        public float Pitch;

        public float Roll;

        public float Yaw;

        public float Speed;
    }

}
