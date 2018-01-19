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

using TeachingPlatformApp.Utils;

namespace TeachingPlatformApp.Controls
{
    /// <summary>
    /// GridAxes.xaml 的交互逻辑
    /// </summary>
    public partial class GridAxes : Grid
    {
        private GridAxesDrawPara drawPara;

        private int columnNum;
        private int rowNum;

        private Color redColor = Color.FromRgb(222, 0, 0);

        private double _xAxesInternal = 100;
        public double XAxesInternal
        {
            get => _xAxesInternal;
            set => _xAxesInternal = value;
        }

        private double _yAxesInternal = 100;
        public double YAxesInternal
        {
            get => _yAxesInternal;
            set => _yAxesInternal = value;
        }

        public int LineStrokeThickness { get; set; } = 2;

        public double DrawTop { get; set; }

        public double DrawLeft { get; set; }

        public GridAxes()
        {
            InitializeComponent();
            DrawParaInit();
            RenewBuildAxes();
            RenewAxesLabel();
        }

        private void DrawParaInit()
        {
            drawPara = JsonFileConfig.ReadFromFile().GridAxesDrawPara;
            columnNum = (int)(drawPara.axesHeight / YAxesInternal) + 1;
            rowNum = (int)(drawPara.axesWidth / XAxesInternal) + 1;
            DrawLeft = drawPara.drawLeft;
            DrawTop = drawPara.drawTop;
        }

        private void RenewAxesLabel()
        {
                      
        }

        private void RenewBuildAxes()
        {

            for (var i = 0; i < rowNum; ++i)
            {
                labelCanvas.Children.Add(new TextBlock()
                {
                    Foreground = new SolidColorBrush(Colors.Black),
                    FontSize = drawPara.labelFontSize,
                    Text = (i * drawPara.labelAxesInterval + drawPara.labelAxesInit).ToString(),
                    Margin = new Thickness(drawPara.drawLabelLeft + i * XAxesInternal,
                        drawPara.drawLabelTop, 0, 0)
                });
                var path = new Path()
                {
                    Stroke = new SolidColorBrush(redColor),
                    StrokeThickness = LineStrokeThickness,
                    Data = new LineGeometry()
                    {
                        StartPoint = new Point(i * XAxesInternal, 0 + DrawTop),
                        EndPoint = new Point(i * XAxesInternal, 
                            drawPara.axesHeight + drawPara.drawDown)
                    }
                };
                chartCanvas.Children.Add(path);
            }
            for (var i = 0; i < columnNum; ++i)
            {
                labelCanvas.Children.Add(new TextBlock()
                {
                    Foreground = new SolidColorBrush(Colors.Black),
                    FontSize = drawPara.labelFontSize,
                    Text = (i * drawPara.labelAxesInterval + drawPara.labelAxesInit).ToString(),
                    Margin = new Thickness(drawPara.drawLabelLeft, 
                        drawPara.drawLabelTop + i * YAxesInternal, 0, 0)
                });
                var path = new Path()
                {
                    Stroke = new SolidColorBrush(redColor),
                    StrokeThickness = LineStrokeThickness,
                    Data = new LineGeometry()
                    {
                        StartPoint = new Point(0 + DrawLeft, i * YAxesInternal),
                        EndPoint = new Point(drawPara.axesWidth + drawPara.drawRight, 
                            i * YAxesInternal)
                    }
                };
                chartCanvas.Children.Add(path);
            }
        }
    }

    public class GridAxesDrawPara
    {
        public double axesHeight = 760;
        public double axesWidth = 960;
        public double drawTop = -30;
        public double drawDown = 30;
        public double drawLeft = -30;
        public double drawRight = 30;
        public double drawLabelTop = 5;
        public double drawLabelLeft = 12;

        public int labelFontSize = 20;
        public int labelAxesInterval = 10;
        public int labelAxesInit = 0;

    }

}
