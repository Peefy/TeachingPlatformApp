﻿using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Navigation;
using MahApps.Metro.Controls;

using TeachingPlatformApp.Utils;
using TeachingPlatformApp.ViewModels;

namespace TeachingPlatformApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        MainWindowViewModel _viewModel;
        bool _isCtrlKeyDown;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainWindowViewModel();
            this.DataContext = _viewModel;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.ScrollToEnd();
        }

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
    }
}
