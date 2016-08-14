using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace OneDo.View.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var test = (bool)value;
            if (parameter != null)
            {
                test = !test;
            }
            return test ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var test = (Visibility)value == Visibility.Visible;
            if (parameter != null)
            {
                test = !test;
            }
            return test;
        }
    }
}

