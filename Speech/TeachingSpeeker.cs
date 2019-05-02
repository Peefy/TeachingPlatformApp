using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

using DotNetSpeech;

using TeachingPlatformApp.Utils;

namespace TeachingPlatformApp.Speech
{
    public class TeachingSpeeker : ISpeek
    {

        public const double MinVolume = 0;
        public const double MaxVolume = 100;

        public const double MinRate = -10;
        public const double MaxRate = 10;

        SpeechSynthesizer _synthesizer;
        SpVoice _speech;
        JsonFileConfig _config;

        bool _isUsingDotnetSpeech = true;

        public SpVoice DotnetSpeech => _speech;

        public TeachingSpeeker()
        {
            _config = JsonFileConfig.Instance;
            _isUsingDotnetSpeech = _config.SpeechConfig.IsUsingDotnetSpeech;
            if (_config.SpeechConfig.SpeechEnable == false)
                return;
            if(_isUsingDotnetSpeech == true)
            {
                _speech = new SpVoice
                {
                    //Volume = (int)NumberUtil.Clamp(_config.SpeechConfig.Volume, MinVolume, MaxVolume),
                    //Rate = (int)NumberUtil.Clamp(_config.SpeechConfig.Rate, MinRate, MaxRate)
                };
            }
            else
            {
                string language = "zh-CN";
                _synthesizer = new SpeechSynthesizer
                {
                    Volume = (int)NumberUtil.Clamp(_config.SpeechConfig.Volume, MinVolume, MaxVolume),
                    Rate = (int)NumberUtil.Clamp(_config.SpeechConfig.Rate, MinRate, MaxRate)
                };
                _synthesizer.SetOutputToDefaultAudioDevice();  
                _synthesizer.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Teen, 0, new CultureInfo(language));
            }

        }

        public void SpeekAsync(string text)
        {
            StopSpeek();
            if (_isUsingDotnetSpeech == true)
            {             
                _speech?.Speak(text, SpeechVoiceSpeakFlags.SVSFlagsAsync);
            }
            else
            {
                _synthesizer?.SpeakAsync(text);
            }
        }

        public void SetSpeechRate(double rate)
        {
            if (_speech == null && _synthesizer == null)
                return;
            if (_isUsingDotnetSpeech == true)
            { 
                _speech.Rate = (int)NumberUtil.Clamp(rate, MinRate, MaxRate);
            }
            else
            {        
                _synthesizer.Rate = (int)NumberUtil.Clamp(rate, MinRate, MaxRate);
            }
        }

        public void SetSpeechVolume(double volume)
        {
            if (_speech == null && _synthesizer == null)
                return;
            if (_isUsingDotnetSpeech == true)
            {
                _speech.Volume = (int)NumberUtil.Clamp(_config.SpeechConfig.Volume, MinVolume, MaxVolume);
            }
            else
            {
                _synthesizer.Volume = (int)NumberUtil.Clamp(_config.SpeechConfig.Volume, MinVolume, MaxVolume);
            }
        }

        public void StopSpeek()
        {
            if (_speech == null && _synthesizer == null)
                return;
            if (_isUsingDotnetSpeech == true)
            {
                _speech?.Speak("", SpeechVoiceSpeakFlags.SVSFlagsAsync);
            }
            else
            {
                _synthesizer?.SpeakAsyncCancelAll();
            }
        }
    }
}
