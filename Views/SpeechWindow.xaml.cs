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

using DotNetSpeech;

namespace TeachingPlatformApp.Views
{
    /// <summary>
    /// SpeechWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SpeechWindow : Window
    {
        SpVoice speech = new SpVoice();
        int speechRate = 0;
        int volume = 70;
        public SpeechWindow()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            //初始化语音引擎列表  
            foreach (ISpeechObjectToken Token in speech.GetVoices(string.Empty, string.Empty))
            {
                cmbVoices.Items.Add(Token.GetDescription(49));
            }
            //取得音频输出列表  
            foreach (ISpeechObjectToken AudioOut in speech.GetAudioOutputs(string.Empty, string.Empty))
            {
                cmbAudioOut.Items.Add(AudioOut.GetDescription(49));
            }

            cmbVoices.SelectedIndex = 0;
            cmbAudioOut.SelectedIndex = 0;
            tbarRate.Value = speechRate;
            trbVolume.Value = volume;

        }

        private void tbarRate_Scroll(object sender, EventArgs e)
        {
            speech.Rate = (int)tbarRate.Value;
        }

        private void trbVolume_Scroll(object sender, EventArgs e)
        {
            speech.Volume = (int)trbVolume.Value;
        }

        private void cmbVoices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            speech.Voice = speech.GetVoices(string.Empty, string.Empty).Item(cmbVoices.SelectedIndex);
        }

        private void cmbAudioOut_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            speech.AudioOutput = speech.GetAudioOutputs(string.Empty, string.Empty).Item(cmbAudioOut.SelectedIndex);
        }

        private void bt_speek_Click(object sender, EventArgs e)
        {
            //终止先前朗读,如果有  
            speech.Speak(" ", SpeechVoiceSpeakFlags.SVSFlagsAsync);
            speech.Speak(tbspeech.Text, SpeechVoiceSpeakFlags.SVSFlagsAsync);
        }

        private void bt_stop_Click(object sender, EventArgs e)
        {
            speech.Speak("", SpeechVoiceSpeakFlags.SVSFlagsAsync);
        }

        private void tbarRate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            speech.Rate = (int)e.NewValue;
        }

        private void trbVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            speech.Volume = (int)e.NewValue;
        }
    }
}
