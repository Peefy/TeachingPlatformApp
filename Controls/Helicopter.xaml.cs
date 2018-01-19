using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Prism.Mvvm;

namespace TeachingPlatformApp.Controls
{
    /// <summary>
    /// Helicopter.xaml 的交互逻辑
    /// </summary>
    public partial class Helicopter : UserControl
    {
        public Helicopter()
        {
            InitializeComponent();
        }
    }

    public class HelicopterViewModel : BindableBase
    {

        private float _helicopterAngle = 0;
        public float HelicopterAngle
        {
            get => _helicopterAngle;
            set => SetProperty(ref _helicopterAngle, value);
        }

        public HelicopterViewModel()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    HelicopterAngle += 2;
                    if (HelicopterAngle >= 360)
                        HelicopterAngle = 0;
                    Thread.Sleep(40);
                }
            });
        }
    }

}
