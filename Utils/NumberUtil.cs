using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachingPlatformApp.Utils
{
    public static class NumberUtil
    {
        /// <summary>
        /// 判断数字是否在范围内
        /// </summary>
        /// <param name="number"></param>
        /// <param name="range1"></param>
        /// <param name="range2"></param>
        /// <returns></returns>
        public static bool JudgeNumberInRange(double number, double range1, double range2)
        {
            var max = Math.Max(range1, range2);
            var min = Math.Min(range1, range2);
            if (number >= min && number <= max)
                return true;
            return false;
        }

        /// <summary>
        /// bool类型转 true -> '是';false ->'否'
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string BoolToString(bool value)
        {
            return value == true ? "否" : "是";
        }

        /// <summary>
        /// 数字限幅
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double Clamp(double value, double min, double max)
        {
            if (value <= min)
                value = min;
            if (value >= max)
                value = max;
            return value;
        }

        /// <summary>
        /// 弧度转变为角度 单精度浮点
        /// </summary>
        /// <param name="rad">输入弧度大小</param>
        /// <param name="digit">小数点后保留digit位。默认2位</param>
        /// <returns></returns>
        public static float Rad2Deg(float rad, int digit = 2)
        {
            return (float)Math.Round(rad * 57.29577951 / Math.PI, digit);
        }

        /// <summary>
        /// 弧度转变为角度 双精度浮点
        /// </summary>
        /// <param name="rad">输入弧度大小</param>
        /// <param name="digit">小数点后保留digit位。默认2位</param>
        /// <returns></returns>
        public static float Rad2Deg(double rad, int digit = 2)
        {
            return (float)Math.Round(rad * 57.29577951 / Math.PI, digit);
        }

        /// <summary>
        /// 角度转变为弧度 单精度浮点
        /// </summary>
        /// <param name="deg">输入角度大小</param>
        /// <param name="digit">小数点后保留digit位。默认2位</param>
        /// <returns></returns>
        public static float Deg2Rad(float deg, int digit = 2)
        {
            return (float)Math.Round(deg * 0.01745329, digit);
        }

        /// <summary>
        /// 角度转变为弧度 双精度浮点
        /// </summary>
        /// <param name="deg">输入角度大小</param>
        /// <param name="digit">小数点后保留digit位。默认2位</param>
        /// <returns></returns>
        public static float Deg2Rad(double deg, int digit = 2)
        {
            return (float)Math.Round(deg * 0.01745329, digit);
        }

        public static double PutAngleIn(float angle, double minAngle = 0, double maxAngle = 360, int digit = 2)
        {
            while(angle <= minAngle)
                angle += 360;
            while (angle >= maxAngle)
                angle -= 360;
            return Math.Round(angle, digit);
        }
    }
}
