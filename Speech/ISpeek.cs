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
        void SpeekWords(string words, bool isAsync = true, string language = "zh-CN");
        IReadOnlyCollection<InstalledVoice> GetInstalledVoices(string language = "zh-CN");
    }
}
