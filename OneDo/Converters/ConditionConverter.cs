using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace OneDo.Converters
{

    public class ConditionConverter : IValueConverter
    {
        public bool Debug { get; set; }

        public object Value { get; set; }

        public object When { get; set; }

        public object Otherwise { get; set; }

        public object OtherwiseValueBack { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (Debug)
            {
                Debugger.Break();
            }

            try
            {
                if (object.Equals(value, parameter ?? When))
                {
                    return Value;
                }
                return Otherwise;
            }
            catch
            {
                return Otherwise;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (Debug)
            {
                Debugger.Break();
            }

            if (OtherwiseValueBack == null)
            {
                throw new InvalidOperationException($"Cannot {nameof(ConvertBack)} if no {nameof(OtherwiseValueBack)} is set!");
            }
            try
            {
                return object.Equals(value, Value) ? When : OtherwiseValueBack;
            }
            catch
            {
                return OtherwiseValueBack;
            }
        }
    }
}
