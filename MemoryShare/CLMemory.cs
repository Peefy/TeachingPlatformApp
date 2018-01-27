using System;
using System.Runtime.InteropServices;

using TeachingPlatformApp.ZyPlatform;

namespace DuGu720DegreeView.ShareMemory
{
    public class CLMemory 
    {
        private const int FileMapHead = 4;

        private static CLMemory _instance;

        private string _memName = "GAME_SHARED_MEM_0000";

        private ZyShareMem _memDB = new ZyShareMem();

        private IntPtr _hMappingHandle = IntPtr.Zero;

        private IntPtr _hVoid = IntPtr.Zero;

        private bool _inited;

        private VectorAngExp _vectorAngExp;

        private GameSharedDef _gameSharedDef;

        public static CLMemory Instance
        {
            get
            {
                return _instance;
            }         
        }

        private void Awake()
        {
            _instance = this;
            this._vectorAngExp = default;
            this._vectorAngExp.X = 0f;
            this._vectorAngExp.Y = 0f;
            this._vectorAngExp.Z = 0f;
            this._gameSharedDef = default;
            this._gameSharedDef.Angle = _vectorAngExp;
            this._gameSharedDef.GameStatus = 0;
        }

        private void Update()
        {
            if (!this._inited)
            {
                this._hMappingHandle = ZyShareMem.OpenFileMapping(4, false, this._memName);
                if (this._hMappingHandle == IntPtr.Zero)
                {
                    return;
                }
                this._hVoid = ZyShareMem.MapViewOfFile(this._hMappingHandle, 4u, 0u, 0u, (uint)Marshal.SizeOf(this._gameSharedDef));
                if (this._hVoid == IntPtr.Zero)
                {
                    return;
                }
                
                this._inited = true;
            }
            if (this._inited)
            {
                this._gameSharedDef = (GameSharedDef)this.ReadFromMemory(this._gameSharedDef.GetType());
                this._vectorAngExp = this._gameSharedDef.Angle;
                
            }
        }

        public int GetGameStatus()
        {
            return this._gameSharedDef.GameStatus;
        }

        public object ReadFromMemory(Type type)
        {
            return Marshal.PtrToStructure(this._hVoid, type);
        }

        private void Close()
        {
            if (this._hVoid != IntPtr.Zero)
            {
                ZyShareMem.UnmapViewOfFile(this._hVoid);
                this._hVoid = IntPtr.Zero;
            }
            if (this._hMappingHandle != IntPtr.Zero)
            {
                ZyShareMem.CloseHandle(this._hMappingHandle);
                this._hMappingHandle = IntPtr.Zero;
            }
        }

        private void OnApplicationQuit()
        {
            this.Close();
        }
    }

}
