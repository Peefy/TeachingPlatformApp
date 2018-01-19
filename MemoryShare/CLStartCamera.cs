

using TeachingPlatformApp.ZyPlatform;

namespace DuGu720DegreeView.ShareMemory
{
    public class CLStartCamera
    {

        private bool m_IsPlay;

        private void Awake()
        {
            
        }

        private void Start()
        {

        }

        private void Update()
        {
            if (CLMemory.Instance.GetGameStatus() == 1 && !this.m_IsPlay)
            //if(true && !this.m_IsPlay)
            {
                this.m_IsPlay = true;
                this.SendBeginData();
            }       
        }

        private void SendBeginData()
        {
            var gameStateFlag = default(GameStateFlag);
            gameStateFlag.m_Flag = 1;
            gameStateFlag.m_length = 6;
            gameStateFlag.m_Type = 1;
        }
    }


}
