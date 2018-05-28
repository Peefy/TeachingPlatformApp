using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using MahApps.Metro.Controls;

using DotNetSpeech;

using TeachingPlatformApp.Speech;

namespace TeachingPlatformApp.Views
{
    /// <summary>
    /// SpeechWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SpeechWindow : MetroWindow
    {
        SpVoice _speech = new SpVoice();
        int _speechRate = 0;
        int _volume = 70;

        public SpeechWindow()
        {
            InitializeComponent();
            Init();
        }

        public SpeechWindow(TeachingSpeeker spVoice)
        {
            InitializeComponent();
            _speech = spVoice.DotnetSpeech;
            Init();
        }

        private void Init()
        {
            
            //初始化语音引擎列表  
            foreach (ISpeechObjectToken Token in _speech.GetVoices(string.Empty, string.Empty))
            {
                cmbVoices.Items.Add(Token.GetDescription(49));
            }
            //取得音频输出列表  
            foreach (ISpeechObjectToken AudioOut in _speech.GetAudioOutputs(string.Empty, string.Empty))
            {
                cmbAudioOut.Items.Add(AudioOut.GetDescription(49));
            }

            cmbVoices.SelectedIndex = 0;
            cmbAudioOut.SelectedIndex = 0;
            tbarRate.Value = _speechRate;
            trbVolume.Value = _volume;

        }

        private void TbarRate_Scroll(object sender, EventArgs e)
        {
            _speech.Rate = (int)tbarRate.Value;
        }

        private void TrbVolume_Scroll(object sender, EventArgs e)
        {
            _speech.Volume = (int)trbVolume.Value;
        }

        private void CmbVoices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _speech.Voice = _speech.GetVoices(string.Empty, string.Empty).Item(cmbVoices.SelectedIndex);
        }

        private void CmbAudioOut_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _speech.AudioOutput = _speech.GetAudioOutputs(string.Empty, string.Empty).Item(cmbAudioOut.SelectedIndex);
        }

        private void Bt_speek_Click(object sender, EventArgs e)
        {
            //终止先前朗读,如果有  
            _speech.Speak(" ", SpeechVoiceSpeakFlags.SVSFlagsAsync);
            _speech.Speak(tbspeech.Text, SpeechVoiceSpeakFlags.SVSFlagsAsync);
        }

        private void Bt_stop_Click(object sender, EventArgs e)
        {
            _speech.Speak("", SpeechVoiceSpeakFlags.SVSFlagsAsync);
        }

        private void TbarRate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _speech.Rate = (int)e.NewValue;
        }

        private void TrbVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _speech.Volume = (int)e.NewValue;
        }
    }
}
