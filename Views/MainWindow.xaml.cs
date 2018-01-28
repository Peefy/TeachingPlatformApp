using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Navigation;
using MahApps.Metro.Controls;

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
                        var text = consoleTextBox.GetLineText(i);
                        if (Regex.IsMatch(text, "speech", RegexOptions.IgnoreCase))
                        {
                            new SpeechWindow().Show();
                            break;
                        }
                        else if (Regex.IsMatch(text, "config", RegexOptions.IgnoreCase))
                        {
                            new ConfigWindow().Show();
                            break;
                        }
                    }
                    
                }
                catch 
                {

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
