using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachingPlatformApp.WswPlatform
{
    public static class WswHelper
    {
        public static string AngleWithLocationToString(AngleWithLocation angle) => 
            $"x:{angle.X} y:{angle.Y} z:{angle.Z} roll:{angle.Roll} yaw:{angle.Yaw} pitch:{angle.Pitch}";

        public static AngleWithLocation MathRoundAngle(AngleWithLocation angle, int digit)
        {
            return new AngleWithLocation
            {
                X = Math.Round(angle.X, digit),
                Y = Math.Round(angle.Y, digit),
                Z = Math.Round(angle.Z, digit),
                Yaw = Math.Round(angle.Yaw, digit),
                Pitch = Math.Round(angle.Pitch, digit),
                Roll = Math.Round(angle.Roll, digit)
            };
        }
    }
}
