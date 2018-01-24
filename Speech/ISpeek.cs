using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace TeachingPlatformApp.Speech
{
    public interface ISpeek
    {
        ///<summary>
        /// 调用系统 语音朗读
        /// </summary>
        /// <param name="words">朗读的内容</param>
        /// <param name="isAsync">是否同步朗读</param>
        /// <param name="language">朗读语言:英语"en-US"，简体中文"zh-CN"，台湾中文"zh-TW"</param>
        void SpeekWords(string words, bool isAsync = true, string language = "zh-CN");
        /// <summary>
        /// 获取所需语言的语言库
        /// </summary>
        /// <param name="language">英语"en-US"，简体中文"zh-CN"，台湾中文"zh-TW"</param>
        /// <returns></returns>
        IReadOnlyCollection<InstalledVoice> GetInstalledVoices(string language = "zh-CN");
    }
}
