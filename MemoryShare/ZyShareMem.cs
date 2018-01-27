using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace DuGu720DegreeView.ShareMemory
{
    public class ZyShareMem
    {
        private const int ERROR_ALREADY_EXISTS = 183;

        private const int FILE_MAP_COPY = 1;

        private const int FILE_MAP_WRITE = 2;

        private const int FILE_MAP_READ = 4;

        private const int FILE_MAP_ALL_ACCESS = 6;

        private const int PAGE_READONLY = 2;

        private const int PAGE_READWRITE = 4;

        private const int PAGE_WRITECOPY = 8;

        private const int PAGE_EXECUTE = 16;

        private const int PAGE_EXECUTE_READ = 32;

        private const int PAGE_EXECUTE_READWRITE = 64;

        private const int SEC_COMMIT = 134217728;

        private const int SEC_IMAGE = 16777216;

        private const int SEC_NOCACHE = 268435456;

        private const int SEC_RESERVE = 67108864;

        private const int INVALID_HANDLE_VALUE = -1;

        private const int infoSize = 50;

        private IntPtr _hSharedMemoryFile = IntPtr.Zero;

        private IntPtr _pwData = IntPtr.Zero;

        private IntPtr _pwDataWrite = IntPtr.Zero;

        private IntPtr _pwDataRead = IntPtr.Zero;

        private bool _bInit;

        private int _length;

        private int _count;

        private Semaphore _semRead;

        private Semaphore _semWrite;

        private Semaphore _semWriteLength;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr CreateFileMapping(int hFile, IntPtr lpAttributes, uint flProtect, uint dwMaxSizeHi, uint dwMaxSizeLow, string lpName);

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr OpenFileMapping(int dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, string lpName);

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr MapViewOfFile(IntPtr hFileMapping, uint dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow, uint dwNumberOfBytesToMap);

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool UnmapViewOfFile(IntPtr pvBaseAddress);

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32")]
        public static extern int GetLastError();

        ~ZyShareMem()
        {
            this.Close();
        }

        public int Init(string strName)
        {
            if (strName.Length <= 0)
            {
                return 1;
            }
            this._hSharedMemoryFile = ZyShareMem.OpenFileMapping(6, false, strName);
            if (this._hSharedMemoryFile == IntPtr.Zero)
            {
                this._bInit = false;
                return 2;
            }
            this._pwData = ZyShareMem.MapViewOfFile(this._hSharedMemoryFile, 6u, 0u, 0u, 0u);
            this._pwDataWrite = this._pwData;
            this._pwDataRead = (IntPtr)(this._pwData.GetHashCode() + 50);
            if (this._pwData == IntPtr.Zero)
            {
                this._bInit = false;
                ZyShareMem.CloseHandle(this._hSharedMemoryFile);
                return 3;
            }
            this._bInit = true;
            this.SetSemaphore();
            return 0;
        }

        public byte[] Read()
        {
            int length = this.GetLength();
            byte[] array = new byte[length];
            if (this._bInit)
            {
                Marshal.Copy(this._pwDataRead, array, 0, this._length);
                return array;
            }
            return null;
        }

        private void Close()
        {
            if (this._bInit)
            {
                UnmapViewOfFile(this._pwData);
                CloseHandle(this._hSharedMemoryFile);
            }
        }

        private bool SetSemaphore()
        {
            try
            {
                this._semRead = Semaphore.OpenExisting("ReadShareMemory");
                this._semWrite = Semaphore.OpenExisting("WriteShareMemory");
                this._semWriteLength = Semaphore.OpenExisting("WriteLengthShareMemory");
            }
            catch (Exception)
            {
                this._semRead = new Semaphore(0, 1, "ReadShareMemory");
                this._semWrite = new Semaphore(1, 1, "WriteShareMemory");
                this._semWriteLength = new Semaphore(1, 1, "WriteLengthShareMemory");
            }
            return true;
        }

        private int ReadLengthAndCount()
        {
            byte[] array = new byte[50];
            if (this._bInit)
            {
                Marshal.Copy(this._pwData, array, 0, 50);
                string input = Encoding.Unicode.GetString(array).Trim(new char[1]);
                string[] array2 = Regex.Split(input, "\0");
                if (int.TryParse(array2[0], out this._length))
                {
                }
                if (int.TryParse(array2[1], out this._count))
                {
                }
                return 0;
            }
            return 1;
        }

        private int GetLength()
        {
            this.ReadLengthAndCount();
            return this._length;
        }

        private int GetCount()
        {
            this.ReadLengthAndCount();
            return this._count;
        }
    }

}
