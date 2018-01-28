using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachingPlatformApp.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// 给字符串前面添加时间，后面添加换行符 (C# string 是值类型 String是引用类型)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="appendText"></param>
        /// <returns></returns>
        public static string AppendTextWithTime(this string text,string appendText)
        {
            text += $"{DateTime.Now} {appendText}{Environment.NewLine}";
            return text;
        }

    }

}
