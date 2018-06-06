namespace TeachingPlatformApp.WswPlatform
{
    /// <summary>
    /// 战斗机，直升机初始坐标设置通信Packet
    /// </summary>
    public struct VSPFlightVisualCommand
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

        public double Engines01;
        public double Engines02;
        public double Engines11;
        public double Engines12;
        public double Engines21;
        public double Engines22;
        public double Engines31;
        public double Engines32;

        public int Locked;

        public Glmdvec3 Position0;
        public Glmdvec3 Position1;

        public double Yaw0;
        public double Yaw1;

        public double Pitch0;
        public double Pitch1;

        public double Roll0;
        public double Roll1;

        public int Hit;

        public int Fire0;
        public int Fire1;

        //public char[] Text = new char[128];

    }

    public struct Glmdvec3
    {
        public double X;
        public double Y;
        public double Z;
    }

}
