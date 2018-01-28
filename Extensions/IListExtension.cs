using System.Collections.Generic;

namespace TeachingPlatformApp.Extensions
{
    public static class IListExtension
    {
        /// <summary>
        /// ToString()一个集合类型 (返回类型类似于Python的str()函数,没有递归调用) 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ToListString<T>(this IList<T> list)
        {
            var str = "[";
            if (list?.Count > 0)
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
