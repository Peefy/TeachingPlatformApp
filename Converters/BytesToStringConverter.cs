using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachingPlatformApp.Converters
{
    public class BytesToStringConverter
    {
        public static string BytesToString(byte[] bytes)
        {
            var str = "";
            if (bytes != null)
            {
                foreach (var b in bytes)
                    str += b.ToString("X2") + " ";
            }
            return str;
        }
    }
}
