using System;
using System.Timers;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;


using MahApps.Metro.Controls;

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
        FlightMapWindowViewModel viewModel;

        Dispatcher _dip;
        SynchronizationContext _ds;
        Grid _girdWswModel;
        CanvasTrail canvasTrailFlighter;
        CanvasTrail canvasTrailHelicopter;
        CanvasTrail canvasTrailMissile;

        int canvasTrailFlighterIndex = 0;
        int canvasTrailHelicopterIndex = 1;
        int canvasTrailMissileIndex = 2;

        bool enableScale = false;
        bool enableDrag = false;
        Point pressPoint = new Point();
        Point movePoint = new Point();

        public FlightMapWindow()
        {
            InitializeComponent();
            _dip = Dispatcher.CurrentDispatcher;
            _ds = new DispatcherSynchronizationContext();
            _girdWswModel = gridAxes.Children[2] as Grid;
            canvasTrailFlighter = _girdWswModel.Children[canvasTrailFlighterIndex] as CanvasTrail;
            canvasTrailHelicopter = _girdWswModel.Children[canvasTrailHelicopterIndex] as CanvasTrail;
            canvasTrailMissile = _girdWswModel.Children[canvasTrailMissileIndex] as CanvasTrail;
            viewModel = new FlightMapWindowViewModel();
            this.DataContext = viewModel;
            DragMoveInit();
            RenewUI();
            TaskInit();
        }

        private void DragMoveInit()
        {
            if(JsonFileConfig.ReadFromFile().GridAxesDrawPara.EnableDragMove == true)
            {
                gridAxes.MouseRightButtonDown += gridAxes_MouseRightButtonDown;
                gridAxes.MouseRightButtonUp += gridAxes_MouseRightButtonUp;
                gridAxes.MouseMove += gridAxes_MouseMove;
            }
        }

        private void TaskInit()
        {
            Task.Run(() =>
            {
                var config = JsonFileConfig.Instance;
                var interval = config.TestTrailRouteConfig.OutOfRouteTestIntervalMs;
                while (true)
                {
                    try
                    {
                        viewModel.JudgeRouteTask();
                        Thread.Sleep(interval);
                    }
                    catch (Exception ex)
                    {
                        LogAndConfig.Log.Error(ex);
                    }
                }
            });
        }

        private async void RenewUI()
        {
            var config = JsonFileConfig.ReadFromFile().GridAxesDrawPara;
            this.Width = config.AxesWidth;
            this.Height = config.AxesHeight;
            await Task.Delay(100);
            var timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(AddPoint);
            timer.Interval = JsonFileConfig.Instance.DataShowConfig.MapUiRefreshMs + 120;
            timer.AutoReset = true; 
            timer.Enabled = true;
        }

        private void AddPoint(object sender, ElapsedEventArgs e)
        {
            try
            {
                _dip.Invoke(new Action(() =>
                {                  
                    canvasTrailFlighter.AddPoint(viewModel.Flighter.MyMapPosition);
                    canvasTrailHelicopter.AddPoint(viewModel.Helicopter.MyMapPosition);
                    canvasTrailMissile.AddPoint(viewModel.Missile.MyMapPosition);
                }));
            }
            catch
            {

            }
        }

        #region WindowKeyAndMouseWheel
        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if(enableScale == true)
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
                enableScale = false;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.C)
            {
                canvasTrailFlighter.ClearPoint();
                canvasTrailHelicopter.ClearPoint();
                canvasTrailMissile.ClearPoint();
            }
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
                enableScale = true;
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
        #endregion

        #region DragAction
        private void gridAxes_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (enableDrag == true)
                return;
            var tmp = (GridAxes)sender;
            pressPoint = e.GetPosition(null);
            tmp.CaptureMouse();
            tmp.Cursor = Cursors.Hand;
            enableDrag = true;
        }

        private void gridAxes_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {         
            var tmp = (GridAxes)sender;
            tmp.Cursor = Cursors.Arrow;
            tmp.ReleaseMouseCapture();
            enableDrag = false;
        }

        private void gridAxes_MouseMove(object sender, MouseEventArgs e)
        {
            if(enableDrag == true && e.RightButton == MouseButtonState.Pressed)
            {
                var tmp = (GridAxes)sender;
                movePoint = e.GetPosition(null);
                tmp.DrawDeltaLeft += (movePoint.X - pressPoint.X);
                tmp.DrawDeltaTop += (movePoint.Y - pressPoint.Y);
                tmp.RenewBuildAxes();
                var dx = (movePoint.X - pressPoint.X) * 1 + viewModel.DrawMargin.Left;
                var dy = (movePoint.Y - pressPoint.Y) * 1 + viewModel.DrawMargin.Top;
                viewModel.DrawMargin = new Thickness(dx, dy, 0, 0);
                pressPoint = movePoint;

            }
        }
        #endregion

        private void NumuricTextBlock_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBlock = sender as NumuricTextBlock;
            var x = textBlock.Text;
            var left = (double)(new PointXYToMarginLeftTop().Convert(x, null, null, null));
            gridAxes.DrawDeltaLeft = left;
            gridAxes.RenewBuildAxes();
            var dx = left;
            var dy = viewModel.DrawMargin.Top;
            viewModel.DrawMargin = new Thickness(dx, dy, 0, 0);
        }

        

    }
}
