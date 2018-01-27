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
                return default;
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

    }
}
