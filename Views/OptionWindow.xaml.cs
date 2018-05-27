using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using MahApps.Metro.Controls;

using TeachingPlatformApp.Utils;
using TeachingPlatformApp.WswPlatform;

namespace TeachingPlatformApp.Views
{
    /// <summary>
    /// OptionWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OptionWindow : MetroWindow
    {
        public OptionWindow()
        {
            InitializeComponent();
            ControlInit();
        }

        private void ControlInit()
        {
            var config = JsonFileConfig.Instance.StringResource;

            wswModelComboBox.Items.Add(config.FlighterName);
            wswModelComboBox.Items.Add(config.Flighter2Name);
            wswModelComboBox.Items.Add(config.HelicopterName);
            wswModelComboBox.SelectedIndex = 0;

            wswModelComboBox1.Items.Add(config.FlighterName);
            wswModelComboBox1.Items.Add(config.Flighter2Name);
            wswModelComboBox1.Items.Add(config.HelicopterName);
            wswModelComboBox1.Items.Add("全部");
            wswModelComboBox1.SelectedIndex = 0;

            var showtexts = JsonFileConfig.ReadFromFile().FlightExperimentConfig.ShowText;
            foreach(var str in showtexts)
            {
                showTextComboBox.Items.Add(str);
            }
            showTextComboBox.SelectedIndex = 0;

            SetXYBlockPositionText(WswModelKind.Flighter);
        }

        private void SetXYBlockPositionText(WswModelKind kind)
        {
            var info = WswHelper.KindToMyInfo(kind);
            xBlock.Text = info.InitMyPointX.ToString();
            yBlock.Text = info.InitMyPointY.ToString();
        }
        
        private void ButtonSetPositionClick(object sender, RoutedEventArgs e)
        {
            var x = Convert.ToSingle(xBlock.Text);
            var y = Convert.ToSingle(yBlock.Text);
            var kind = IndexToModelKind(wswModelComboBox.SelectedIndex);
            var info = WswHelper.KindToMyInfo(kind);
            PositionCommandBuilder.SendPositionTo(kind, x, y);
            info.InitMyPointX = x;
            info.InitMyPointY = y;      
        }

        private WswModelKind IndexToModelKind(int index)
        {
            if (index == 0)
                return WswModelKind.Flighter;
            else if (index == 1)
                return WswModelKind.Flighter2;
            else if (index == 2)
                return WswModelKind.Helicopter;
            return WswModelKind.Missile;
        }

        private void WswModelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = sender as ComboBox;
            var kind = IndexToModelKind(combo.SelectedIndex);
            SetXYBlockPositionText(kind);            
        }
        
        private void MetroWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                //ButtonSetPositionClick(btnSetPosition, null);
                btnSetShowText_Click(null, null);
            }
        }

        private void ButtonInitPositionClick(object sender, RoutedEventArgs e)
        {
            var kind = IndexToModelKind(wswModelComboBox.SelectedIndex);
            PositionCommandBuilder.SetDefaultPositionTo(kind);
        }

        private void btnSetShowText_Click(object sender, RoutedEventArgs e)
        {
            var time = int.Parse(textShowTime.Text);
            var text = textShowText.Text;
            // 给全部模型发送显示文字
            if (wswModelComboBox1.SelectedIndex == 3)
            {
                ShowTextCommandBuilder.SetShowTextTo(WswModelKind.Helicopter, text, time);
                ShowTextCommandBuilder.SetShowTextTo(WswModelKind.Flighter, text, time);
                ShowTextCommandBuilder.SetShowTextTo(WswModelKind.Flighter2, text, time);
                return;
            }
            var kind = IndexToModelKind(wswModelComboBox1.SelectedIndex);           
            try
            {
                ShowTextCommandBuilder.SetShowTextTo(kind, text, time);
            }
            catch 
            {             
                MessageBox.Show($"不超过{ShowTextCommandBuilder.TextMaxLength / 2}个汉字!");
            }     
        }

        private void showTextComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = sender as ComboBox;
            textShowText.Text = combo.SelectedItem.ToString();
        }
    }
}
