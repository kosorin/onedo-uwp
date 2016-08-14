using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace OneDo.View.Converters
{
    public class HexToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var hex = value as string;
            var r = byte.Parse(hex.Substring(1, 2), NumberStyles.HexNumber);
            var g = byte.Parse(hex.Substring(3, 2), NumberStyles.HexNumber);
            var b = byte.Parse(hex.Substring(5, 2), NumberStyles.HexNumber);

            var color = Color.FromArgb(255, r, g, b);
            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
