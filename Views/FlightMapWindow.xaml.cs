using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

using MahApps.Metro.Controls;

using DuGu.NetFramework.Services;

using TeachingPlatformApp.Speech;
using TeachingPlatformApp.Utils;
using TeachingPlatformApp.ViewModels;
using TeachingPlatformApp.Controls;
using TeachingPlatformApp.Converters;

namespace TeachingPlatformApp.Views
{
    /// <summary>
    /// Interaction logic for FlightMapWindow.xaml
    /// </summary>
    public partial class FlightMapWindow : MetroWindow
    {
        FlightMapWindowViewModel _viewModel;

        Dispatcher _dip;
        SynchronizationContext _ds;
        Grid _girdWswModel;
        CanvasTrail _canvasTrailFlighter;
        CanvasTrail _canvasTrailHelicopter;
        CanvasTrail _canvasTrailMissile;

        int _canvasTrailFlighterIndex = 0;
        int _canvasTrailHelicopterIndex = 1;
        int _canvasTrailMissileIndex = 2;

        bool _enableScale = false;
        bool _enableDrag = false;
        Point _pressPoint = new Point();
        Point _movePoint = new Point();

        Thread _speechThread;

        int _autoFollowingMouseClickCount = 2;
        int _renewUiStartDelay = 100;

        public FlightMapWindow()
        {
            InitializeComponent();
            DataInit();           
            DragMoveInit();
            RenewUI();
            TaskInit();
        }

        private void DataInit()
        {
            _dip = Dispatcher.CurrentDispatcher;
            _ds = new DispatcherSynchronizationContext();
            _girdWswModel = gridAxes.Children[2] as Grid;
            _canvasTrailFlighter = _girdWswModel.Children[_canvasTrailFlighterIndex] as CanvasTrail;
            _canvasTrailHelicopter = _girdWswModel.Children[_canvasTrailHelicopterIndex] as CanvasTrail;
            _canvasTrailMissile = _girdWswModel.Children[_canvasTrailMissileIndex] as CanvasTrail;
            _viewModel = new FlightMapWindowViewModel();
            DataContext = _viewModel;
        }

        private void DragMoveInit()
        {
            if(JsonFileConfig.ReadFromFile().GridAxesDrawPara.EnableDragMove == true)
            {
                gridAxes.MouseLeftButtonDown += GridAxes_MouseLeftButtonDown;
                gridAxes.MouseRightButtonDown += GridAxes_MouseRightButtonDown;
                gridAxes.MouseRightButtonUp += GridAxes_MouseRightButtonUp;
                gridAxes.MouseMove += GridAxes_MouseMove;
            }
        }

        private void TaskInit()
        {
            _speechThread = new Thread(new ThreadStart(() =>
            {
                var config = JsonFileConfig.Instance;
                var outOfRouteTestInterval = config.TestTrailRouteConfig.OutOfRouteTestIntervalMs;
                while (true)
                {
                    try
                    {
                        _viewModel.JudgeRouteTask();
                        Thread.Sleep(outOfRouteTestInterval);
                    }
                    catch (Exception ex)
                    {
                        LogAndConfig.Log.Error(ex);
                    }
                }
            }));
            if(JsonFileConfig.Instance.SpeechConfig.SpeechEnable == true)
                _speechThread.Start();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if(_speechThread != null)
            {
                _speechThread.Abort();
                _speechThread = null;
                Ioc.Get<ISpeek>().StopSpeek();
            }
        }

        private async void RenewUI()
        {
            var config = JsonFileConfig.ReadFromFile().GridAxesDrawPara;
            this.Width = config.AxesWidth;
            this.Height = config.AxesHeight;
            //延时一下
            await Task.Delay(_renewUiStartDelay);
            var timerAddPoint = new System.Timers.Timer();
            timerAddPoint.Elapsed += new ElapsedEventHandler(AddPoint);
            timerAddPoint.Interval = JsonFileConfig.Instance.DataShowConfig.MapUiRefreshMs + 120;
            timerAddPoint.AutoReset = true; 
            timerAddPoint.Enabled = true;

            var timerAutoFollowing = new System.Timers.Timer();
            timerAutoFollowing.Elapsed += new ElapsedEventHandler(AutoFollowing);
            timerAutoFollowing.Interval = JsonFileConfig.Instance.TestTrailRouteConfig.AutoFollowingIntervalMs;
            timerAutoFollowing.AutoReset = true;
            timerAutoFollowing.Enabled = true;
        }

        private void AddPoint(object sender, ElapsedEventArgs e)
        {
            try
            {
                _dip.Invoke(new Action(() =>
                {                  
                    _canvasTrailFlighter.AddPoint(_viewModel.Flighter.MyMapPosition);
                    _canvasTrailHelicopter.AddPoint(_viewModel.Helicopter.MyMapPosition);
                    _canvasTrailMissile.AddPoint(_viewModel.Missile.MyMapPosition);

                }));
            }
            catch(Exception ex)
            {
                LogAndConfig.Log.Error(ex);
            }
        }

        private void AutoFollowing(object sender, ElapsedEventArgs e)
        {
            try
            {
                _dip.Invoke(new Action(() =>
                {
                    var isAutoFollowing = JsonFileConfig.Instance.TestTrailRouteConfig.IsAutoFollowing;
                    if (isAutoFollowing == 1)
                    {
                        SetMapDrawDeltaLeftTop(_viewModel.Flighter.MyMapPosition);
                    }
                    if (isAutoFollowing == 2)
                    {
                        SetMapDrawDeltaLeftTop(_viewModel.Helicopter.MyMapPosition);
                    }
                }));
            }
            catch(Exception ex) 
            {
                LogAndConfig.Log.Error(ex);
            }

        }

        #region WindowKeyAndMouseWheel
        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if(_enableScale == true)
            {
                var delta = e.Delta;
                if (fatherGrid.RenderTransform is ScaleTransform scale)
                {
                    scale.ScaleX += delta * 0.001;
                    scale.ScaleY += delta * 0.001;
                }
            }

        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
                _enableScale = false;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.C)
            {
                _canvasTrailFlighter.ClearPoint();
                _canvasTrailHelicopter.ClearPoint();
                _canvasTrailMissile.ClearPoint();
            }
            if(e.Key == Key.A)
            {
                if (++JsonFileConfig.Instance.TestTrailRouteConfig.IsAutoFollowing > 2)
                    JsonFileConfig.Instance.TestTrailRouteConfig.IsAutoFollowing = 0;
            }
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
            if(e.Key == Key.Enter)
            {
                TextChanged(null, null);
            }
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
                _enableScale = true;
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

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            var converter = new PointXYToMarginLeftTop();
            var x = xTextBox.Text;
            var y = yTextBox.Text;
            var left = converter.Convert(x);
            var top = converter.Convert(y);
            gridAxes.DrawDeltaLeft = left;
            gridAxes.DrawDeltaTop = top;
            gridAxes.RenewBuildAxes(this.Width, this.Height);
            var dx = _viewModel.DrawMargin.Left;
            var dy = _viewModel.DrawMargin.Top;
            _viewModel.DrawMargin = new Thickness(dx, dy, 0, 0);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            gridAxes.RenewBuildAxes(this.Width, this.Height, true);
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            gridAxes.RenewBuildAxes(this.Width, this.Height, true);

        }

        #endregion

        #region DragAction

        public void SetMapDrawDeltaLeftTop(Point point)
        {
            var x = JsonFileConfig.Instance.GridAxesDrawPara.MouseDoubleClickShowPointX;
            var y = JsonFileConfig.Instance.GridAxesDrawPara.MouseDoubleClickShowPointY;
            var convert = new PointXYToMarginLeftTop();
            var left = convert.Convert((x - point.X ).ToString());
            var top = convert.Convert((y - point.Y).ToString()) ;
            gridAxes.DrawDeltaLeft = left;
            gridAxes.DrawDeltaTop = top;
            gridAxes.RenewBuildAxes(this.Width, this.Height, true);
            _viewModel.DrawMargin = new Thickness(left, top, 0, 0);
        }

        private void GridAxes_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var tmp = (GridAxes)sender;
            if (e.ClickCount == _autoFollowingMouseClickCount)
            {
                SetMapDrawDeltaLeftTop(_viewModel.Flighter.MyMapPosition);
            }
        }

        private void GridAxes_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var tmp = (GridAxes)sender;
            if (e.ClickCount == _autoFollowingMouseClickCount)
            {
                SetMapDrawDeltaLeftTop(_viewModel.Helicopter.MyMapPosition);
            }
            else
            {
                if (_enableDrag == true)
                    return;
                _pressPoint = e.GetPosition(null);
                tmp.CaptureMouse();
                tmp.Cursor = Cursors.Hand;
                _enableDrag = true;
            }
        }

        private void GridAxes_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {         
            var tmp = (GridAxes)sender;
            tmp.Cursor = Cursors.Arrow;
            tmp.ReleaseMouseCapture();
            _enableDrag = false;
        }

        private void GridAxes_MouseMove(object sender, MouseEventArgs e)
        {
            if(_enableDrag == true && e.RightButton == MouseButtonState.Pressed)
            {
                var mouseMoveScale = 1.0f;
                var tmp = (GridAxes)sender;
                _movePoint = e.GetPosition(null);
                tmp.DrawDeltaLeft += (_movePoint.X - _pressPoint.X);
                tmp.DrawDeltaTop += (_movePoint.Y - _pressPoint.Y);
                tmp.RenewBuildAxes(this.Width, this.Height, true);
                var dx = (_movePoint.X - _pressPoint.X) * mouseMoveScale + _viewModel.DrawMargin.Left;
                var dy = (_movePoint.Y - _pressPoint.Y) * mouseMoveScale + _viewModel.DrawMargin.Top;
                _viewModel.DrawMargin = new Thickness(dx, dy, 0, 0);
                _pressPoint = _movePoint;

            }
        }
        #endregion


    }
}
