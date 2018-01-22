using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace TeachingPlatformApp.Utils
{
    public static class StructHelper
    {
        public static object BytesToStruct(byte[] bytes, Type type)
        {
            int num = Marshal.SizeOf(type);
            if (num > bytes.Length)
            {
                return null;
            }
            IntPtr intPtr = Marshal.AllocHGlobal(num);
            Marshal.Copy(bytes, 0, intPtr, num);
            object result = Marshal.PtrToStructure(intPtr, type);
            Marshal.FreeHGlobal(intPtr);
            return result;
        }

        public static T BytesToStruct<T>(byte[] bytes)
        {
            var type = typeof(T);
            int num = Marshal.SizeOf(type);
            if (num > bytes.Length)
            {
                return default(T);
            }
            IntPtr intPtr = Marshal.AllocHGlobal(num);
            Marshal.Copy(bytes, 0, intPtr, num);
            var result = Marshal.PtrToStructure(intPtr, type);
            Marshal.FreeHGlobal(intPtr);
            return (T)result;
        }

        public static byte[] StructToBytes(object structObj)
        {
            int num = Marshal.SizeOf(structObj);
            byte[] array = new byte[num];
            IntPtr intPtr = Marshal.AllocHGlobal(num);
            Marshal.StructureToPtr(structObj, intPtr, false);
            Marshal.Copy(intPtr, array, 0, num);
            Marshal.FreeHGlobal(intPtr);
            return array;
        }

        public static int GetStructSize<T>()
        {
            var type = typeof(T);
            int num = Marshal.SizeOf(type);
            return num;
        }

        public static float Rad2Deg(float rad, int digit = 2)
        {
            return (float)Math.Round(rad * 57.29577951 / Math.PI, digit);
        }

        public static float Rad2Deg(double rad, int digit = 2)
        {
            return (float)Math.Round(rad * 57.29577951 / Math.PI, digit);
        }

        public static float Deg2Rad(float deg, int digit = 2)
        {
            return (float)Math.Round(deg * 0.01745329, digit);
        }

        public static float Deg2Rad(double deg, int digit = 2)
        {
            return (float)Math.Round(deg * 0.01745329, digit);
        }

    }
}
