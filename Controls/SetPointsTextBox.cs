using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TeachingPlatformApp.Controls
{
    /// <summary>
    /// 航路点输入控件
    /// </summary>
    public class SetPointsTextBox : NumuricTextBlock
    {

        public SetPointsTextBox()
        {
            this.Resources.Add("HintText", new VisualBrush()
            {
                TileMode = TileMode.None,
                Opacity = 0.5,
                Stretch = Stretch.None,
                AlignmentX = AlignmentX.Left,
                Visual = new TextBlock()
                {
                    Text = "航路点为空，请按规定格式输入航路点坐标"
                }
            });
            var style = new Style(typeof(SetPointsTextBox));
            var trigger = new Trigger()
            {
                Property = TextProperty,
                Value = "",
            };
            trigger.Setters.Add(new Setter()
            {
                Property = BackgroundProperty,
                Value = this.Resources["HintText"]
            });
            style.Triggers.Add(trigger);
            this.Style = style;
        }

        public override bool IsNumberic(string _string)
        {
            if (string.IsNullOrEmpty(_string))
                return false;
            foreach (char c in _string)
            {
                if (!char.IsDigit(c) && !(c == ',') && !(c == '(') && !(c == ')'))
                    return false;
            }
            return true;
        }
    }
}
