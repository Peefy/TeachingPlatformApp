using System.Collections;
using System.Windows;
using System.Windows.Controls;

using TeachingPlatformApp.Models;

namespace TeachingPlatformApp.UIModels
{
    /// <summary>
    /// ToTalFlightView.xaml 的交互逻辑
    /// </summary>
    public partial class ToTalFlightView : Grid
    {

        public int IndexShow
        {
            get => (int)GetValue(IndexShowProperty);
            set => SetValue(IndexShowProperty, value);
        }

        public IEnumerable ItemViews
        {
            get => (IEnumerable)GetValue(ItemViewsProperty);
            set => SetValue(ItemViewsProperty, value);
        }

        public static readonly DependencyProperty IndexShowProperty =
             DependencyProperty.Register("IndexShow", typeof(int),
                 typeof(ToTalFlightView), new PropertyMetadata(0,
                     IndexShowPropertyChanged));

        public static void IndexShowPropertyChanged(DependencyObject sender,DependencyPropertyChangedEventArgs e)
        {
            var grid = sender as ToTalFlightView;
            grid.Init((int)e.NewValue);
            if((int)e.NewValue == -1)
            {
                grid.Children[grid.Children.Count - 1].Visibility = Visibility.Visible;
            }
        }

        public static readonly DependencyProperty ItemViewsProperty =
             DependencyProperty.Register("ItemViews", typeof(IEnumerable),
                 typeof(ToTalFlightView), new PropertyMetadata(null,
                     ItemViewsPropertyChanged));

        public static void ItemViewsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var grid = sender as ToTalFlightView;
            if(e.NewValue is ObservableRangeCollection<FlightExperiment> items)
            {
                for(var i = 0; i< items.Count;++i)
                {
                    var childGrid = grid.Children[i] as Grid;
                    childGrid.DataContext = items[i];
                }
            }
        }

        public ToTalFlightView()
        {
            InitializeComponent();
            Init(0);
            Children[Children.Count - 1].Visibility = Visibility.Visible;
            
        }

        public void Init(int index)
        {
            for(int i = 0;i<Children.Count;++i)
            {
                Children[i].Visibility = Visibility.Collapsed;
            }
            if (index < 0 || index >= Children.Count)
                return;
            Children[index].Visibility = Visibility.Visible;
        }
    }
}
