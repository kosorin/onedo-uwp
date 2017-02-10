using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace OneDo.View.Converters
{
    [ContentProperty(Name = nameof(Converters))]
    public class CompositeConverter : IValueConverter
    {
        public Collection<IValueConverter> Converters { get; } = new Collection<IValueConverter>();

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            foreach (var converter in Converters)
            {
                value = converter.Convert(value, targetType, parameter, language);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            foreach (var converter in Converters.Reverse())
            {
                value = converter.ConvertBack(value, targetType, parameter, language);
            }
            return value;
        }
    }
}
