using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls;

using TeachingPlatformApp.Utils;

namespace TeachingPlatformApp.Views
{
    /// <summary>
    /// Interaction logic for FlightMapWindow.xaml
    /// </summary>
    public partial class FlightMapWindow : MetroWindow
    {

        public FlightMapWindow()
        {
            InitializeComponent();
            RenewUI();
        }

        private void RenewUI()
        {
            var config = JsonFileConfig.ReadFromFile().GridAxesDrawPara;
            this.Width = config.axesWidth;
            this.Height = config.axesHeight;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (fatherGrid.RenderTransform is ScaleTransform scale)
            {
                if (e.Key == Key.PageUp || e.Key == Key.W)
                {
                    scale.ScaleX += 0.1;
                    scale.ScaleY += 0.1;
                }
                else if (e.Key == Key.PageDown || e.Key == Key.S)
                {
                    scale.ScaleX -= 0.1;
                    scale.ScaleY -= 0.1;
                }
            }
        }
    }
}
