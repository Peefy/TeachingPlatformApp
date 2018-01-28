using System.Windows;
using System.Windows.Input;
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
        private GridAxesDrawPara _drawPara;

        private double _marginWild = 20;
        private int _columnNum;
        private int _rowNum;

        private Color _redColor = Color.FromRgb(222, 0, 0);

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

        public double DrawDeltaTop { get; set; } = 0;

        public double DrawDeltaLeft { get; set; } = 0;

        public GridAxes()
        {
            InitializeComponent();
            DrawParaInit();
            RenewBuildAxes(_drawPara.AxesWidth, _drawPara.AxesHeight, true);
        }

        private void DrawParaInit()
        {
            _drawPara = JsonFileConfig.ReadFromFile().GridAxesDrawPara;
            _columnNum = _drawPara.ColumnNumber;
            _rowNum = _drawPara.RowNumber;
            DrawLeft = _drawPara.DrawLeft;
            DrawTop = _drawPara.DrawTop;
            XAxesInternal = _drawPara.XAxesInternal;
            YAxesInternal = _drawPara.YAxesInternal;
        }

        private bool JudgeLeftTopInRange(double left, double top, double width, double height)
        {
            if (left >= -_marginWild && left <= width + _marginWild && 
                top >= -_marginWild && top <= height + _marginWild)
                return true;
            return false;
        }

        public void RenewBuildAxes(double width, double height, bool isUseMax = false)
        {
            if(isUseMax == true)
            {
                width = _drawPara.MaxWidth;
                height = _drawPara.MaxHeight;
            }
            labelCanvas.Children.Clear();
            chartCanvas.Children.Clear();
            for (var i = -_rowNum; i < _rowNum; ++i)
            {
                var left = _drawPara.DrawLabelLeft + i * XAxesInternal + DrawDeltaLeft - 5;
                var top = _drawPara.DrawLabelTop + 5;
                if (JudgeLeftTopInRange(left, top, width, height) == true)
                {
                    labelCanvas.Children.Add(new TextBlock()
                    {
                        Foreground = new SolidColorBrush(Colors.Black),
                        FontSize = _drawPara.LabelFontSize,
                        Text = (i * _drawPara.LabelAxesInterval + _drawPara.LabelAxesInit).ToString(),
                        Margin = new Thickness(left, top + 5, 0, 0)
                    });
                }
                left = i * XAxesInternal + DrawDeltaLeft;
                if (JudgeLeftTopInRange(left, top, width, height) == true)
                {
                    var path = new Path()
                    {
                        Stroke = new SolidColorBrush(_redColor),
                        StrokeThickness = LineStrokeThickness,
                        Data = new LineGeometry()
                        {
                            StartPoint = new Point(left, 0 + DrawTop),
                            EndPoint = new Point(left, height
                                + _drawPara.DrawDown)
                        }
                    };
                    chartCanvas.Children.Add(path);
                }
            }
            for (var i = -_columnNum; i < _columnNum; ++i)
            {
                var left = _drawPara.DrawLabelLeft + 5;
                var top = _drawPara.DrawLabelTop + i * YAxesInternal + DrawDeltaTop - 5;
                if (JudgeLeftTopInRange(left, top, width, height) == true)
                {
                    labelCanvas.Children.Add(new TextBlock()
                    {
                        Foreground = new SolidColorBrush(Colors.Black),
                        FontSize = _drawPara.LabelFontSize,
                        Text = (i * _drawPara.LabelAxesInterval + _drawPara.LabelAxesInit).ToString(),
                        Margin = new Thickness(left, top, 0, 0)
                    });
                }
                top = i * YAxesInternal + DrawDeltaTop;
                if (JudgeLeftTopInRange(left, top, width, height) == true)
                {
                    var path = new Path()
                    {
                        Stroke = new SolidColorBrush(_redColor),
                        StrokeThickness = LineStrokeThickness,
                        Data = new LineGeometry()
                        {
                            StartPoint = new Point(0 + DrawLeft, top),
                            EndPoint = new Point(width + _drawPara.DrawRight, top)
                        }
                    };
                    chartCanvas.Children.Add(path);
                }
            }
        }

    }

    public class GridAxesDrawPara
    {
        [JsonProperty("axesHeight")]
        public double AxesHeight { get; set; } = 860;

        [JsonProperty("axesWidth")]
        public double AxesWidth { get; set; } = 1360;

        [JsonProperty("maxHeight")]
        public double MaxHeight { get; set; } = 1000;

        [JsonProperty("maxWidth")]
        public double MaxWidth { get; set; } = 2000;

        [JsonProperty("columnNumber")]
        public int ColumnNumber { get; set; } = 200;

        [JsonProperty("rowNumber")]
        public int RowNumber { get; set; } = 200;

        [JsonProperty("enableDragMove")]
        public bool EnableDragMove { get; set; } = true;

        [JsonProperty("drawTop")]
        public double DrawTop { get; set; } = -30;

        [JsonProperty("drawDown")]
        public double DrawDown { get; set; } = 30;

        [JsonProperty("drawLeft")]
        public double DrawLeft { get; set; } = -30;

        [JsonProperty("drawRight")]
        public double DrawRight { get; set; } = 60;

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

        [JsonProperty("mouseDoubleClickShowPointX")]
        public double MouseDoubleClickShowPointX { get; set; } = 50;

        [JsonProperty("mouseDoubleClickShowPointY")]
        public double MouseDoubleClickShowPointY { get; set; } = 30;

        [JsonProperty("helicopterPaddleRotateSpeed")]
        public float HelicopterPaddleRotateSpeed { get; set; } = 4.0f;
    }

}
