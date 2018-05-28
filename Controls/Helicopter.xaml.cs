using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Prism.Mvvm;

using TeachingPlatformApp.Utils;

namespace TeachingPlatformApp.Controls
{
    /// <summary>
    /// Helicopter.xaml 的交互逻辑
    /// </summary>
    public partial class Helicopter : UserControl
    {
        Dispatcher _dip;
        SynchronizationContext _ds;

        private float _rotateSpeed;

        private float _angle = 0;

        public bool IsEnablePaddleRotate { get; set; }

        System.Timers.Timer timerRotate;

        public Helicopter()
        {
            InitializeComponent();
            _dip = Dispatcher.CurrentDispatcher;
            _ds = new DispatcherSynchronizationContext();
            if(IsEnablePaddleRotate == true)
            {
                BuildPaddleRotateTimer();
            }
            UpdateColor(Colors.DarkSlateGray);
        }

        Color color;
        public Color Color => color;
        public void UpdateColor(Color color)
        {
            if (canvas.Children[0] is Ellipse eliipse)
            {
                this.color = color;
                eliipse.Fill = new SolidColorBrush(color);
            }
        }

        public bool IsChangeColor => color == Colors.Purple;

        public void BuildPaddleRotateTimer()
        {
            if(timerRotate == null)
            {
                timerRotate = new System.Timers.Timer();
                timerRotate.Elapsed += new ElapsedEventHandler(PaddleRotate);
                timerRotate.Interval = 50;
                timerRotate.AutoReset = true;
                timerRotate.Enabled = true;
            }
        }

        private void PaddleRotate(object sender, ElapsedEventArgs e)
        {
            try
            {
                _rotateSpeed = JsonFileConfig.Instance.GridAxesDrawPara.HelicopterPaddleRotateSpeed;
                _dip.Invoke(new Action(() =>
                {                   
                    _angle += _rotateSpeed;
                    if (_angle >= 360)
                        _angle = 0;
                    ChangePathRotateTransform(path1, _angle);
                    ChangePathRotateTransform(path2, _angle);
                    ChangePathRotateTransform(path3, _angle);
                    ChangePathRotateTransform(path4, _angle);
                }));
            }
            catch (Exception ex)
            {
                LogAndConfig.Log.Error(ex);
            }
        }

        private void ChangePathRotateTransform(Path path, float angle)
        {
            if (path.RenderTransform is RotateTransform rotate)
            {
                rotate.Angle = angle;
            }
        }

        public WswPlatform.WswModelKind Kind => WswPlatform.WswModelKind.Helicopter;

    }

}
