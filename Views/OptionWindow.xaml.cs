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
            var info = KindToinfo(kind);
            xBlock.Text = info.InitMyPointX.ToString();
            yBlock.Text = info.InitMyPointY.ToString();
        }

        private WswModelInfo KindToinfo(WswModelKind kind)
        {
            WswModelInfo info = default;
            if (kind == WswModelKind.Flighter)
                info = JsonFileConfig.Instance.MyFlighterInfo;
            else if (kind == WswModelKind.Flighter2)
                info = JsonFileConfig.Instance.MyFlighter2Info;
            else if (kind == WswModelKind.Helicopter)
                info = JsonFileConfig.Instance.MyHelicopterInfo;
            else
                info = JsonFileConfig.Instance.MyMissileInfo;
            return info;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PositionCommandBuilder.SendPositionTo(WswModelKind.Flighter, 20, 20);
            PositionCommandBuilder.SendPositionTo(WswModelKind.Flighter, 20, 20);         
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var x = Convert.ToSingle(xBlock.Text);
            var y = Convert.ToSingle(yBlock.Text);
            var kind = IndexToModelKind(wswModelComboBox.SelectedIndex);
            var info = KindToinfo(kind);
            PositionCommandBuilder.SendPositionTo(kind, x, y);
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

        private void wswModelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = sender as ComboBox;
            var kind = IndexToModelKind(combo.SelectedIndex);
            SetXYBlockPositionText(kind);
        }
    }
}
