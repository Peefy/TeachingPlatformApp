using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace TeachingPlatformApp.Controls
{
    /// <summary>
    /// Flighter.xaml 的交互逻辑
    /// </summary>
    public partial class Flighter : UserControl
    {

        public Flighter()
        {
            InitializeComponent();
        }

        Color color;
        public Color Color => color;
        public void UpdateColor(Color color)
        {
            this.color = color;
            foreach(var children in canvas.Children)
            {
                if(children is Path path)
                {
                    path.Fill = new SolidColorBrush(color);
                    path.Stroke = new SolidColorBrush(color);
                }
            }
        }

        public bool IsChangeColor => color == Colors.Purple;

        public virtual WswPlatform.WswModelKind Kind => WswPlatform.WswModelKind.Flighter;

    }

}
