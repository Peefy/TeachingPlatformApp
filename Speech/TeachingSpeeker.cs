using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace TeachingPlatformApp.Speech
{
    public class TeachingSpeeker : ISpeek
    {
        ///<summary>
        /// 调用系统 语音朗读
        /// </summary>
        /// <param name="words">朗读的内容</param>
        /// <param name="isAsync">是否同步朗读</param>
        /// <param name="language">朗读语言:英语"en-US"，简体中文"zh-CN"，台湾中文"zh-TW"</param>
        public void SpeekWords(string words, bool isAsync = true, string language = "zh-CN")
        {
            using (SpeechSynthesizer synthesizer = new SpeechSynthesizer())
            {
                synthesizer.Volume = 100;   //音量 0~100   最大只能100
                synthesizer.Rate = 0;   //  语速 -10~10    0 中等
                synthesizer.SetOutputToDefaultAudioDevice();

                //var voices = synthesizer.GetInstalledVoices(new CultureInfo(language));
                synthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult, 0, new CultureInfo(language));
                //synthesizer.Speak("hello word 你好");
                if (isAsync)
                {
                    //异步朗读
                    synthesizer.SpeakAsync(words);
                }
                else
                {  //同步朗读
                    synthesizer.Speak(words);
                }
            }
        }

        public IReadOnlyCollection<InstalledVoice> GetInstalledVoices(string language = "zh-CN")
        {
            using (var synthesizer = new SpeechSynthesizer())
            {
                return synthesizer.GetInstalledVoices(new CultureInfo(language));
            }
        }

    }
}
