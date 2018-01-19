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

namespace TeachingPlatformApp.UIModels
{
    /// <summary>
    /// TakeOffLandingView.xaml 的交互逻辑
    /// </summary>
    public partial class TakeOffLandingView : Grid
    {
        public TakeOffLandingView()
        {
            InitializeComponent();
            
        }

        private void SetPointsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text == "")
                textBox.Text = "(";
        }
    }
}
