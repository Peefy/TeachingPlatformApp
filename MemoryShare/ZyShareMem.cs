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

        private IntPtr m_hSharedMemoryFile = IntPtr.Zero;

        private IntPtr m_pwData = IntPtr.Zero;

        private IntPtr m_pwDataWrite = IntPtr.Zero;

        private IntPtr m_pwDataRead = IntPtr.Zero;

        private bool m_bAlreadyExist;

        private bool m_bInit;

        private long m_MemSize;

        private int m_length;

        private int m_count;

        private Semaphore semRead;

        private Semaphore semWrite;

        private Semaphore semWriteLength;

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
            this.m_hSharedMemoryFile = ZyShareMem.OpenFileMapping(6, false, strName);
            if (this.m_hSharedMemoryFile == IntPtr.Zero)
            {
                this.m_bAlreadyExist = false;
                this.m_bInit = false;
                return 2;
            }
            this.m_bAlreadyExist = true;
            this.m_pwData = ZyShareMem.MapViewOfFile(this.m_hSharedMemoryFile, 6u, 0u, 0u, 0u);
            this.m_pwDataWrite = this.m_pwData;
            this.m_pwDataRead = (IntPtr)(this.m_pwData.GetHashCode() + 50);
            if (this.m_pwData == IntPtr.Zero)
            {
                this.m_bInit = false;
                ZyShareMem.CloseHandle(this.m_hSharedMemoryFile);
                return 3;
            }
            this.m_bInit = true;
            this.SetSemaphore();
            return 0;
        }

        public byte[] Read()
        {
            int length = this.GetLength();
            byte[] array = new byte[length];
            if (this.m_bInit)
            {
                Marshal.Copy(this.m_pwDataRead, array, 0, this.m_length);
                return array;
            }
            return null;
        }

        private void Close()
        {
            if (this.m_bInit)
            {
                ZyShareMem.UnmapViewOfFile(this.m_pwData);
                ZyShareMem.CloseHandle(this.m_hSharedMemoryFile);
            }
        }

        private bool SetSemaphore()
        {
            try
            {
                this.semRead = Semaphore.OpenExisting("ReadShareMemory");
                this.semWrite = Semaphore.OpenExisting("WriteShareMemory");
                this.semWriteLength = Semaphore.OpenExisting("WriteLengthShareMemory");
            }
            catch (Exception)
            {
                this.semRead = new Semaphore(0, 1, "ReadShareMemory");
                this.semWrite = new Semaphore(1, 1, "WriteShareMemory");
                this.semWriteLength = new Semaphore(1, 1, "WriteLengthShareMemory");
            }
            return true;
        }

        private int ReadLengthAndCount()
        {
            byte[] array = new byte[50];
            if (this.m_bInit)
            {
                Marshal.Copy(this.m_pwData, array, 0, 50);
                string input = Encoding.Unicode.GetString(array).Trim(new char[1]);
                string[] array2 = Regex.Split(input, "\0");
                if (int.TryParse(array2[0], out this.m_length))
                {
                }
                if (int.TryParse(array2[1], out this.m_count))
                {
                }
                return 0;
            }
            return 1;
        }

        private int GetLength()
        {
            this.ReadLengthAndCount();
            return this.m_length;
        }

        private int GetCount()
        {
            this.ReadLengthAndCount();
            return this.m_count;
        }
    }

}
