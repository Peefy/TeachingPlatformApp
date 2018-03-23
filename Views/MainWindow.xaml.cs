using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

using MahApps.Metro.Controls;

using DuGu.NetFramework.Services;

using TeachingPlatformApp.Utils;
using TeachingPlatformApp.ViewModels;
using TeachingPlatformApp.Speech;

namespace TeachingPlatformApp.Views
{
    /// <summary>
    /// 软件主窗体
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        /// 绑定的ViewModel
        /// </summary>
        MainWindowViewModel _viewModel;

        /// <summary>
        /// 键盘的Ctrl按键是否按下
        /// </summary>
        bool _isCtrlKeyDown;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            UIInit();
            _viewModel = new MainWindowViewModel();
            // 绑定ViewModel
            this.DataContext = _viewModel;
            
        }

        /// <summary>
        /// UI 初始化
        /// </summary>
        private void UIInit()
        {
            btnShow.Visibility = JsonFileConfig.Instance.WindowUiConfig.IsInitShowOn == true ?
                Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// 控制台软件显示字符发生变化就将滚动条滚到最底
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.ScrollToEnd();
        }

        /// <summary>
        /// 按键按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
                _isCtrlKeyDown = true;
            if (e.Key == Key.C && _isCtrlKeyDown == true)
            {
                if(DataContext is MainWindowViewModel viewModel)
                {
                    viewModel.ClearCommand.Execute();                 
                }
            }
            else if(e.Key == Key.S && _isCtrlKeyDown == true)
            {
                consoleTextBox.FontSize -= 2;
            }
            else if(e.Key == Key.W && _isCtrlKeyDown == true)
            {
                consoleTextBox.FontSize += 2;
            }
            else if(e.Key == Key.T && _isCtrlKeyDown == true)
            {
                if (testButton.Visibility == Visibility.Collapsed)
                    testButton.Visibility = Visibility.Visible;
                else
                    testButton.Visibility = Visibility.Collapsed;
            }
            else if(e.Key == Key.Enter)
            {
                try
                {
                    var count = consoleTextBox.LineCount;
                    for(var i = 0;i < count ;++i)
                    {
                        var isFind = false;
                        var text = consoleTextBox.GetLineText(i);
                        if (Regex.IsMatch(text, "speech", RegexOptions.IgnoreCase))
                        {
                            _viewModel.OpenSpeechWindowCommand.Execute();
                            _viewModel.AppendStatusText("打开语音助手界面");
                            isFind = true;
                        }
                        if (Regex.IsMatch(text, "speeker", RegexOptions.IgnoreCase))
                        {
                            var speeker = Ioc.Get<ISpeek>() as TeachingSpeeker;
                            new SpeechWindow(speeker).Show();
                            _viewModel.AppendStatusText("打开语音助手界面");
                            isFind = true;
                        }
                        if (Regex.IsMatch(text, "config", RegexOptions.IgnoreCase))
                        {
                            _viewModel.OpenConfigWindowCommand.Execute();
                            _viewModel.AppendStatusText("打开软件配置界面");
                            isFind = true;
                        }
                        if (Regex.IsMatch(text, "(clc)|(clr)|(clear)", RegexOptions.IgnoreCase))
                        {
                            _viewModel.ClearCommand.Execute();
                            isFind = true;
                        }
                        if (Regex.IsMatch(text, "(show)(\\s*)(off)", RegexOptions.IgnoreCase))
                        { 
                            btnShow.Visibility = Visibility.Collapsed;
                            _viewModel.AppendStatusText("关闭显示");
                            isFind = true;
                        }
                        if (Regex.IsMatch(text, "(show)(\\s*)(on)", RegexOptions.IgnoreCase))
                        {
                            btnShow.Visibility = Visibility.Visible;
                            _viewModel.AppendStatusText("打开显示");                      
                            isFind = true;
                        }
                        if (Regex.IsMatch(text, "start", RegexOptions.IgnoreCase))
                        {
                            _viewModel.StartCommand.Execute();
                            isFind = true;
                        }
                        if (Regex.IsMatch(text, "stop", RegexOptions.IgnoreCase))
                        {
                            _viewModel.StopCommand.Execute();
                            isFind = true;
                        }
                        if (Regex.IsMatch(text, "map", RegexOptions.IgnoreCase))
                        {
                            _viewModel.OpenFlightMapCommand.Execute();
                            isFind = true;
                        }
                        if (isFind == true)
                            break;                          
                    }
                    
                }
                catch (Exception ex)
                {
                    LogAndConfig.Log.Error(ex);
                }
                
            }
        }

        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
                _isCtrlKeyDown = false;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Height <= 850)
                btnShow.Visibility = Visibility.Collapsed;
            else
                btnShow.Visibility = Visibility.Visible;
        }
    }
}
