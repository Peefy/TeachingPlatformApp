
using TeachingPlatformApp.Utils;

namespace TeachingPlatformApp.WswPlatform
{
    public class DataPacketToWswBuilder
    {

        protected DataPacketToWsw packet;
        protected JsonFileConfig config;

        private DataPacketToWswBuilder() => packet = new DataPacketToWsw();

        public DataPacketToWswBuilder(int index, double x, double y)
        {
            var info = config.MyFlighterInfo;
            var wswData = config.WswData.FlighterInitInfo;
            packet = new DataPacketToWsw
            {
                Header = 0xCC,
                Index = (byte)index,
                X = (x - info.InitMyPointX) / info.PointScaleFactorX + wswData.X,
                Y = (y - info.InitMyPointY) / info.PointScaleFactorY + wswData.Y
            };
        }

        public DataPacketToWsw Build() => packet;

        public byte[] BuildBytes() => StructHelper.StructToBytes(Build());

    }

}
