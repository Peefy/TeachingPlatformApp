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
        
        /// <summary>
        /// DotnetSpeech.dll and System.Speech change config file to change mode.
        /// </summary>
        /// <param name="text"></param>
        void SpeekAsync(string text);

        /// <summary>
        /// DotnetSpeech.dll and System.Speech change config file to change mode.
        /// </summary>
        /// <param name="rate"></param>
        void SetSpeechRate(double rate);

        /// <summary>
        /// DotnetSpeech.dll and System.Speech change config file to change mode.
        /// </summary>
        /// <param name="volume"></param>
        void SetSpeechVolume(double volume);

        /// <summary>
        /// DotnetSpeech.dll and System.Speech change config file to change mode.
        /// </summary>
        /// <param name="volume"></param>
        void StopSpeek();

    }
}
