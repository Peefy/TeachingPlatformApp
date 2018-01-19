using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachingPlatformApp.Extensions
{
    public static class StringExtension
    {
        public static void AppendTextWithTime(this string text,string appendText)
        {
            text += $"{DateTime.Now} {appendText}{Environment.NewLine}";
        }

        public static void ClearText(this string text)
        {
            text = "";
        }

    }
}
