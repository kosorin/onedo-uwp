using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace OneDo.View.Converters
{
    public class DoubleToGridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return new GridLength((double)value, GridUnitType.Pixel);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
