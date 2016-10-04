using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace OneDo.View.Converters
{
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime? && value != null)
            {
                var date = (value as DateTime?).Value.Date;

                if (date.Date == DateTime.Today)
                {
                    return "Today";
                }
                else if (date.Date == DateTime.Today.AddDays(1))
                {
                    return "Tomorrow";
                }
                else if (date.Date == DateTime.Today.AddDays(-1))
                {
                    return "Yesterday";
                }
                else if (date.Date == DateTime.Today.AddDays(-1))
                {
                    return "Yesterday";
                }
                else if (date.Date > DateTime.Today.AddDays(1) && date.Date < DateTime.Today.AddDays(7))
                {
                    return date.ToString("dddd");
                }
                return date.ToString("d");
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
