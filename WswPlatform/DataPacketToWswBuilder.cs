
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
            config = JsonFileConfig.Instance;
            var info = config.MyFlighterInfo;
            var wswData = config.WswData.FlighterInitInfo;
            packet = new DataPacketToWsw
            {
                Header = 100,
                Index = index,
                X = (x - info.InitMyPointX) / info.PointScaleFactorX + wswData.X,
                Y = (y - info.InitMyPointY) / info.PointScaleFactorY + wswData.Y
            };
        }

        public DataPacketToWsw Build() => packet;

        public byte[] BuildBytes() => StructHelper.StructToBytes(Build());

    }

}
