using System;

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

        /// <summary>
        /// 将角度范围限制在一个2*pi周期内
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="minAngle"></param>
        /// <param name="maxAngle"></param>
        /// <param name="digit"></param>
        /// <returns></returns>
        public static double PutAngleIn(double angle, double minAngle = 0, double maxAngle = 360, int digit = 2)
        {
            if (Math.Round(maxAngle - minAngle) != 360)
            {
                throw new ArgumentException("parameter 'maxAngle' sub parameter 'minAngle' must be 360 deg");
            }
            while (angle <= minAngle)
                angle += 360;
            while (angle >= maxAngle)
                angle -= 360;
            return Math.Round(angle, digit);
        }

        /// <summary>
        /// 0到10的阿拉伯数字转汉字字符串
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string IntNumberToChineseString(int num)
        {
            var str = "";
            switch(num)
            {
                case 0:
                    str = "零";
                    break;
                case 1:
                    str = "一";
                    break;
                case 2:
                    str = "二";
                    break;
                case 3:
                    str = "三";
                    break;
                case 4:
                    str = "四";
                    break;
                case 5:
                    str = "五";
                    break;
                case 6:
                    str = "六";
                    break;
                case 7:
                    str = "七";
                    break;
                case 8:
                    str = "八";
                    break;
                case 9:
                    str = "九";
                    break;
                case 10:
                    str = "十";
                    break;
            }
            return str;
        }

    }
}
