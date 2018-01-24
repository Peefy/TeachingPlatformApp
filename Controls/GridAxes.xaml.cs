using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using Newtonsoft.Json;

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
            columnNum = (int)(drawPara.AxesHeight / YAxesInternal) + 1;
            rowNum = (int)(drawPara.AxesWidth / XAxesInternal) + 1;
            DrawLeft = drawPara.DrawLeft;
            DrawTop = drawPara.DrawTop;
            XAxesInternal = drawPara.XAxesInternal;
            YAxesInternal = drawPara.YAxesInternal;
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
                    FontSize = drawPara.LabelFontSize,
                    Text = (i * drawPara.LabelAxesInterval + drawPara.LabelAxesInit).ToString(),
                    Margin = new Thickness(drawPara.DrawLabelLeft + i * XAxesInternal,
                        drawPara.DrawLabelTop, 0, 0)
                });
                var path = new Path()
                {
                    Stroke = new SolidColorBrush(redColor),
                    StrokeThickness = LineStrokeThickness,
                    Data = new LineGeometry()
                    {
                        StartPoint = new Point(i * XAxesInternal, 0 + DrawTop),
                        EndPoint = new Point(i * XAxesInternal, 
                            drawPara.AxesHeight + drawPara.DrawDown)
                    }
                };
                chartCanvas.Children.Add(path);
            }
            for (var i = 0; i < columnNum; ++i)
            {
                labelCanvas.Children.Add(new TextBlock()
                {
                    Foreground = new SolidColorBrush(Colors.Black),
                    FontSize = drawPara.LabelFontSize,
                    Text = (i * drawPara.LabelAxesInterval + drawPara.LabelAxesInit).ToString(),
                    Margin = new Thickness(drawPara.DrawLabelLeft, 
                        drawPara.DrawLabelTop + i * YAxesInternal, 0, 0)
                });
                var path = new Path()
                {
                    Stroke = new SolidColorBrush(redColor),
                    StrokeThickness = LineStrokeThickness,
                    Data = new LineGeometry()
                    {
                        StartPoint = new Point(0 + DrawLeft, i * YAxesInternal),
                        EndPoint = new Point(drawPara.AxesWidth + drawPara.DrawRight, 
                            i * YAxesInternal)
                    }
                };
                chartCanvas.Children.Add(path);
            }
        }
    }

    public class GridAxesDrawPara
    {
        [JsonProperty("axesHeight")]
        public double AxesHeight { get; set; } = 860;

        [JsonProperty("axesWidth")]
        public double AxesWidth { get; set; } = 1360;

        [JsonProperty("drawTop")]
        public double DrawTop { get; set; } = -30;

        [JsonProperty("drawDown")]
        public double DrawDown { get; set; } = 30;

        [JsonProperty("drawLeft")]
        public double DrawLeft { get; set; } = -30;

        [JsonProperty("drawRight")]
        public double DrawRight { get; set; } = 30;

        [JsonProperty("drawLabelTop")]
        public double DrawLabelTop { get; set; } = 5;

        [JsonProperty("drawLabelLeft")]
        public double DrawLabelLeft { get; set; } = 12;

        [JsonProperty("labelFontSize")]
        public int LabelFontSize { get; set; } = 24;

        [JsonProperty("labelAxesInterval")]
        public int LabelAxesInterval { get; set; } = 10;
        
        [JsonProperty("labelAxesInit")]
        public int LabelAxesInit { get; set; } = 0;

        [JsonProperty("xAxesInternal")]
        public double XAxesInternal { get; set; } = 100;

        [JsonProperty("yAxesInternal")]
        public double YAxesInternal { get; set; } = 100;

    }

}
