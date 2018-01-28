using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachingPlatformApp.Utils
{
    /// <summary>
    /// 集合元素ToString()函数(仿Python)
    /// </summary>
    public class CollectionToStringUtil
    {
        public static string ToString<T>(IList<T> list)
        {
            var str = "[";
            if(list?.Count > 0)
            {
                var count = list.Count;
                for (var i = 0; i < count; ++i)
                    str += list[i] + " ";
            }
            str += "]";
            return str;
        }
    }
}
