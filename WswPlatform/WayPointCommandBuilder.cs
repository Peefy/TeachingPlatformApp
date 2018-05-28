
using TeachingPlatformApp.Utils;

namespace TeachingPlatformApp.WswPlatform
{
    /// <summary>
    /// 航路点设置通信Packet构造器
    /// </summary>
    public class WayPointCommandBuilder
    {

        protected WayPointCommand packet;
        protected JsonFileConfig config;

        private WayPointCommandBuilder() => packet = new WayPointCommand();

        public WayPointCommandBuilder(int index, double x, double y)
        {
            config = JsonFileConfig.Instance;
            var info = config.MyFlighterInfo;
            var wswData = config.WswData.FlighterInitInfo;
            packet = new WayPointCommand
            {
                Header = 100,
                Index = index,
                X = (x - info.InitMyPointX) / info.PointScaleFactorX + wswData.X,
                Y = (y - info.InitMyPointY) / info.PointScaleFactorY + wswData.Y
            };
        }

        public WayPointCommand Build() => packet;

        public byte[] BuildBytes() => StructHelper.StructToBytes(Build());

    }

}
