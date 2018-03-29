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
using TeachingPlatformApp.WswPlatform;

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
        CanvasTrail _canvasTrailFlighter2;
        CanvasTrail _canvasTrailHelicopter;
        CanvasTrail _canvasTrailMissile;
        Helicopter helicopter;
        Flighter flighter;
        Flighter2 flighter2;
        Missile missile;
        int _canvasTrailFlighterIndex = 0;
        int _canvasTrailFlighter2Index = 1;
        int _canvasTrailHelicopterIndex = 2;
        int _canvasTrailMissileIndex = 3;
        int _helicopterIndex = 4;
        int _flighterIndex = 5;
        int _flighter2Index = 6;
        int _missileIndex = 7;

        int _wswModelCount = 3;

        bool _enableMoveModel = false;
        WswModelKind _willMoveKind = WswModelKind.Missile;

        bool _enableScale = false;
        bool _enableDrag = false;
        bool _isCtrlDown = false;
        Point _pressMoveModelPoint = new Point();
        Point _pressPoint = new Point();
        Point _movePoint = new Point();
        Point _pressScalePoint = new Point();
        Point _moveScalePoint = new Point();

        Thread _speechThread;

        int _autoFollowingMouseClickCount = 2;
        int _renewUiStartDelay = 100;

        double _scaleFactor = 0.01;
        double _isHoverOnMinDistance = 2.5;

        public FlightMapWindow()
        {
            InitializeComponent();
            DataInit();           
            DragMoveInit();
            ChangeWswModelScale(JsonFileConfig.Instance.DataShowConfig.WswModelScaleFactor);
            RenewUI();
            TaskInit();
        }

        private void DataInit()
        {
            _dip = Dispatcher.CurrentDispatcher;
            _ds = new DispatcherSynchronizationContext();
            _girdWswModel = gridAxes.Children[2] as Grid;
            _canvasTrailFlighter = _girdWswModel.Children[_canvasTrailFlighterIndex] as CanvasTrail;
            _canvasTrailFlighter2 = _girdWswModel.Children[_canvasTrailFlighter2Index] as CanvasTrail;
            _canvasTrailHelicopter = _girdWswModel.Children[_canvasTrailHelicopterIndex] as CanvasTrail;
            _canvasTrailMissile = _girdWswModel.Children[_canvasTrailMissileIndex] as CanvasTrail;
            helicopter = _girdWswModel.Children[_helicopterIndex] as Helicopter;
            flighter = _girdWswModel.Children[_flighterIndex] as Flighter;
            flighter2 = _girdWswModel.Children[_flighter2Index] as Flighter2;
            missile = _girdWswModel.Children[_missileIndex] as Missile;
            helicopter.BuildPaddleRotateTimer();
            _viewModel = new FlightMapWindowViewModel();
            DataContext = _viewModel;
        }

        private void DragMoveInit()
        {
            if(JsonFileConfig.ReadFromFile().GridAxesDrawPara.EnableDragMove == true)
            {              
                gridAxes.MouseLeftButtonDown += GridAxes_MouseLeftButtonDown;
                gridAxes.MouseLeftButtonUp += GridAxes_MouseLeftButtonUp;
                gridAxes.MouseRightButtonDown += GridAxes_MouseRightButtonDown;
                gridAxes.MouseRightButtonUp += GridAxes_MouseRightButtonUp;
                gridAxes.MouseDown += GridAxes_MouseDown;
                gridAxes.MouseUp += GridAxes_MouseUp;
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
            myHelicopeter.BuildPaddleRotateTimer();
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
                    _canvasTrailFlighter2.AddPoint(_viewModel.Flighter2.MyMapPosition);
                    _canvasTrailHelicopter.AddPoint(_viewModel.Helicopter.MyMapPosition);
                    //_canvasTrailMissile.AddPoint(_viewModel.Missile.MyMapPosition);

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
                    if (isAutoFollowing == 3)
                    {
                        SetMapDrawDeltaLeftTop(_viewModel.Flighter2.MyMapPosition);
                    }
                }));
            }
            catch(Exception ex) 
            {
                LogAndConfig.Log.Error(ex);
            }
        }

        #region WindowKeyAndMouseWheel

        private void MapChangeScale(double delta, bool isAcc = true)
        {
            var config = JsonFileConfig.Instance.GridAxesDrawPara;
            if (fatherGrid.RenderTransform is ScaleTransform scale)
            {
                _canvasTrailFlighter.ClearPoint();
                _canvasTrailHelicopter.ClearPoint();
                _canvasTrailFlighter2.ClearPoint();
                _canvasTrailMissile.ClearPoint();
                if(isAcc == true)
                {
                    config.XAxesInternal += delta * _scaleFactor;
                    config.YAxesInternal += delta * _scaleFactor;
                }
                else
                {
                    config.XAxesInternal = delta * _scaleFactor;
                    config.YAxesInternal = delta * _scaleFactor;
                }
                var scaleHundred = config.XAxesInternal;
                config.LabelFontSize = NumberUtil.Clamp(scaleHundred * 0.24, 12, 30);
                config.AxexLineWidth = config.XAxesInternal * 0.02;
                ChangeWswModelScale(config.XAxesInternal / 100.0);
                ConverterPara.Init();
                gridAxes.DrawParaInit();
                gridAxes.RenewBuildAxes(this.Width, this.Height, true);
                _viewModel.RefreshSetPoints();
                JsonFileConfig.Instance.DataShowConfig.SetPointsLineWidth
                    = scaleHundred * 0.03;
                JsonFileConfig.Instance.DataShowConfig.SetPointsFontSize
                    = NumberUtil.Clamp(scaleHundred * 0.24, 12, 30);
                JsonFileConfig.Instance.DataShowConfig.SetPointsEllipseRadius
                    = scaleHundred * 0.06;
                _viewModel.SetDrawPara();
            }
        }
        
        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var config = JsonFileConfig.Instance.GridAxesDrawPara;
            if(_enableScale == true)
            {
                MapChangeScale(e.Delta);
            }
        }

        private void ChangeWswModelScale(double scale)
        {
            JsonFileConfig.Instance.DataShowConfig.WswModelScaleFactor = scale;
            var tran = (flighter.RenderTransform as TransformGroup).Children[0] as ScaleTransform;
            tran.ScaleX = scale;
            tran.ScaleY = scale;
            tran = (flighter2.RenderTransform as TransformGroup).Children[0] as ScaleTransform;
            tran.ScaleX = scale;
            tran.ScaleY = scale;
            tran = (helicopter.RenderTransform as TransformGroup).Children[0] as ScaleTransform;
            tran.ScaleX = scale;
            tran.ScaleY = scale;
            tran = (missile.RenderTransform as TransformGroup).Children[0] as ScaleTransform;
            tran.ScaleX = scale;
            tran.ScaleY = scale;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                _enableScale = false;
                _isCtrlDown = false;
            }
               
        }

        int _setMapLeftTopKindCount = 0;
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.C)
            {
                _canvasTrailFlighter.ClearPoint();
                _canvasTrailHelicopter.ClearPoint();
                _canvasTrailFlighter2.ClearPoint();
                _canvasTrailMissile.ClearPoint();
            }
            if(e.Key == Key.A)
            {
                if (++JsonFileConfig.Instance.TestTrailRouteConfig.IsAutoFollowing > _wswModelCount)
                    JsonFileConfig.Instance.TestTrailRouteConfig.IsAutoFollowing = 0;
            }
            if(e.Key == Key.Q)
            {
                if(_setMapLeftTopKindCount == 0)
                    SetMapDrawDeltaLeftTop(_viewModel.Flighter.MyMapPosition);
                else if(_setMapLeftTopKindCount == 1)
                    SetMapDrawDeltaLeftTop(_viewModel.Helicopter.MyMapPosition);
                else if(_setMapLeftTopKindCount == 2)
                    SetMapDrawDeltaLeftTop(_viewModel.Flighter2.MyMapPosition);
                if (++_setMapLeftTopKindCount >= 3)
                    _setMapLeftTopKindCount = 0;
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
            {
                _enableScale = true;
                _isCtrlDown = true;
            }
            if(e.Key == Key.T && _isCtrlDown == true)
            {
                _viewModel.RunTest();
            }
            if(e.Key == Key.W)
            {
                MapChangeScale(30);
            }
            if(e.Key == Key.S)
            {
                MapChangeScale(-30);
            }
            if (fatherGrid.RenderTransform is ScaleTransform scale)
            {
                if (e.Key == Key.PageUp)
                {
                    scale.ScaleX += 0.1;
                    scale.ScaleY += 0.1;
                }
                else if (e.Key == Key.PageDown)
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
            var scale = JsonFileConfig.Instance.GridAxesDrawPara.XAxesInternal;
            var x = JsonFileConfig.Instance.GridAxesDrawPara.MouseDoubleClickShowPointX / scale * 100;
            var y = JsonFileConfig.Instance.GridAxesDrawPara.MouseDoubleClickShowPointY / scale * 100;
            var convert = new PointXYToMarginLeftTop();
            var left = convert.Convert((x - point.X ).ToString());
            var top = convert.Convert((y - point.Y).ToString());
            gridAxes.DrawDeltaLeft = left;
            gridAxes.DrawDeltaTop = top;
            gridAxes.RenewBuildAxes(this.Width, this.Height, true);
            _viewModel.DrawMargin = new Thickness(left, top, 0, 0);
        }


        private void GridAxes_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var tmp = (GridAxes)sender;
            if (_enableMoveModel == true && 
                   _willMoveKind != WswModelKind.Missile)
            {
                tmp.Cursor = Cursors.Arrow;
                tmp.ReleaseMouseCapture();
                var mouseMapPoint = MarginPointToMapPointConverter.
                    To(e.GetPosition(null), _viewModel.DrawMargin);             
                var info = WswHelper.KindToinfo(_willMoveKind);
                PositionCommandBuilder.SendPositionTo(_willMoveKind, mouseMapPoint);
                info.InitMyPointX = (float)mouseMapPoint.X;
                info.InitMyPointY = (float)mouseMapPoint.Y;
                _enableMoveModel = false;
                _viewModel.RefreshSetPoints();
                ClearTrail(_willMoveKind);
                ClearTrail(_willMoveKind);
            }  
        }

        public void ClearTrail(WswModelKind kind)
        {
            if (kind == WswModelKind.Flighter)
                _canvasTrailFlighter.ClearPoint();
            if (kind == WswModelKind.Flighter2)
                _canvasTrailFlighter2.ClearPoint();
            if (kind == WswModelKind.Helicopter)
                _canvasTrailHelicopter.ClearPoint();
        }

        private void GridAxes_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var tmp = (GridAxes)sender;
            if (e.ClickCount == _autoFollowingMouseClickCount)
            {
                SetMapDrawDeltaLeftTop(_viewModel.Flighter.MyMapPosition);
            }
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                if (_enableMoveModel == true)
                    return;
                _pressMoveModelPoint = e.GetPosition(null);
                tmp.CaptureMouse();
                tmp.Cursor = Cursors.Hand;
                _enableMoveModel = true;
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


        private void GridAxes_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var tmp = (GridAxes)sender;
            if (e.MiddleButton == MouseButtonState.Released)
            {
                tmp.ReleaseMouseCapture();
                _enableScale = false;
            }
        }

        private void GridAxes_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var tmp = (GridAxes)sender;
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                _pressScalePoint = e.GetPosition(null);
                tmp.CaptureMouse();
                _enableScale = true;
            }
        }

        public bool JudgeMouseIsHoverOnWswModel(WswModelKind kind, Point mouseMapPoint)
        {
            Point point = default;
            if (kind == WswModelKind.Flighter)
            {
                point = _viewModel.Flighter.MyMapPosition;
            }
            else if (kind == WswModelKind.Flighter2)
            {
                point = _viewModel.Flighter2.MyMapPosition;
            }
            else if (kind == WswModelKind.Helicopter)
            {
                point = _viewModel.Helicopter.MyMapPosition;
            }
            else
                return false;
            var distance = VectorPointHelper.GetTwoPointDistance(point, mouseMapPoint);
            return distance < _isHoverOnMinDistance;
        }

        public void UpdateWswModelFillColor(Point mouseMapPoint)
        {
            if (_enableMoveModel == true)
                return;
            var result = JudgeMouseIsHoverOnWswModel(WswModelKind.Flighter, mouseMapPoint);
            flighter.UpdateColor(result == true ? Colors.Purple : Colors.DodgerBlue);
            result = JudgeMouseIsHoverOnWswModel(WswModelKind.Flighter2, mouseMapPoint);
            flighter2.UpdateColor(result == true ? Colors.Purple : Colors.DarkBlue);
            result = JudgeMouseIsHoverOnWswModel(WswModelKind.Helicopter, mouseMapPoint);
            helicopter.UpdateColor(result == true ? Colors.Purple : Colors.DodgerBlue);
            if (flighter.IsChangeColor == true)
                _willMoveKind = WswModelKind.Flighter;
            else if (flighter2.IsChangeColor == true)
                _willMoveKind = WswModelKind.Flighter2;
            else if (helicopter.IsChangeColor == true)
                _willMoveKind = WswModelKind.Helicopter;
            else
                _willMoveKind = WswModelKind.Missile;
        }

        private void GridAxes_MouseMove(object sender, MouseEventArgs e)
        {
            var mouseMapPoint = MarginPointToMapPointConverter.
                To(e.GetPosition(null), _viewModel.DrawMargin);
            UpdateWswModelFillColor(mouseMapPoint);
            if (_enableDrag == true && e.RightButton == MouseButtonState.Pressed)
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
            if(_enableScale == true && e.MiddleButton == MouseButtonState.Pressed)
            {
                _moveScalePoint = e.GetPosition(null);
                var scale = 2;
                var delta = ( _pressScalePoint.Y - _moveScalePoint.Y) * scale;
                MapChangeScale(delta);
                _pressScalePoint = _moveScalePoint;

            }

        }
        #endregion


    }
}
