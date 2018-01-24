using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachingPlatformApp.Utils
{
    public static class NumberUtil
    {
        public static bool JudgeNumberInRange(double number, double range1, double range2)
        {
            var max = Math.Max(range1, range2);
            var min = Math.Min(range1, range2);
            if (number >= min && number <= max)
                return true;
            return false;
        }

        public static string BoolToString(bool value)
        {
            return value == true ? "否" : "是";
        }

        public static double Clamp(double value, double min, double max)
        {
            if (value <= min)
                value = min;
            if (value >= max)
                value = max;
            return value;
        }

    }
}
