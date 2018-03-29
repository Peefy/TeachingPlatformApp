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

using TeachingPlatformApp.Utils;
using TeachingPlatformApp.Utils.JsonModels;
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
            SetXYBlockPositionText(WswModelKind.Flighter);
        }

        private void SetXYBlockPositionText(WswModelKind kind)
        {
            var info = WswHelper.KindToinfo(kind);
            xBlock.Text = info.InitMyPointX.ToString();
            yBlock.Text = info.InitMyPointY.ToString();
        }
        
        private void ButtonSetPositionClick(object sender, RoutedEventArgs e)
        {
            var x = Convert.ToSingle(xBlock.Text);
            var y = Convert.ToSingle(yBlock.Text);
            var kind = IndexToModelKind(wswModelComboBox.SelectedIndex);
            var info = WswHelper.KindToinfo(kind);
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
                ButtonSetPositionClick(btnSetPosition, null);
            }
        }
    }
}
