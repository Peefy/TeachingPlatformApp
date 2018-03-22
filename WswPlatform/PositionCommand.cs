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

    /// <summary>
    /// 战斗机，直升机初始坐标设置通信Packet 构造器
    /// </summary>
    public class PositionCommandBuilder
    {
        PositionCommand _command;

        public PositionCommandBuilder(WswModelKind kind)
        {
            _command = new PositionCommand();
        }

        public PositionCommandBuilder SetInitialPosition(double x, double y, double z)
        {
            _command.MsgType = 0xAA;
            _command.X = x;
            _command.Y = y;
            _command.Z = z;
            return this;
        }

        public PositionCommand Build() => _command;

        public byte[] BuildCommandBytes() => Utils.StructHelper.StructToBytes(_command);

    }
}
