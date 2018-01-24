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

        public FlightMapWindow()
        {
            InitializeComponent();
            _dip = Dispatcher.CurrentDispatcher;
            _ds = new DispatcherSynchronizationContext();
            viewModel = new FlightMapWindowViewModel();
            this.DataContext = viewModel;
            RenewUI();
            TaskInit();
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

            await Task.Delay(200);
            var timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(AddPoint);
            timer.Interval = JsonFileConfig.Instance.DataShowConfig.MapUiRefreshMs;
            timer.AutoReset = true; 
            timer.Enabled = true;
        }

        private void AddPoint(object sender, ElapsedEventArgs e)
        {
            try
            {
                _dip.Invoke(new Action(() =>
                {
                    var canvasTrailFlighter = gridAxes.Children[1] as CanvasTrail;
                    var canvasTrailHelicopter = gridAxes.Children[2] as CanvasTrail;
                    canvasTrailFlighter.AddPoint(viewModel.FlighterPosition);
                    canvasTrailHelicopter.AddPoint(viewModel.HelicopterPosition);
                }));
            }
            catch
            {

            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.C)
            {
                var canvasTrailFlighter = gridAxes.Children[1] as CanvasTrail;
                var canvasTrailHelicopter = gridAxes.Children[2] as CanvasTrail;
                canvasTrailFlighter.ClearPoint();
                canvasTrailHelicopter.ClearPoint();
            }
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
