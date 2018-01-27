

using TeachingPlatformApp.ZyPlatform;

namespace DuGu720DegreeView.ShareMemory
{
    public class CLStartCamera
    {

        private bool _isPlay;

        private void Awake()
        {
            
        }

        private void Start()
        {

        }

        private void Update()
        {
            if (CLMemory.Instance.GetGameStatus() == 1 && !this._isPlay)
            //if(true && !this.m_IsPlay)
            {
                this._isPlay = true;
                this.SendBeginData();
            }       
        }

        private void SendBeginData()
        {
            var gameStateFlag = default(GameStateFlag);
            gameStateFlag.Flag = 1;
            gameStateFlag.Length = 6;
            gameStateFlag.Type = 1;
        }
    }


}
