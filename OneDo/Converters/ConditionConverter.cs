using System;
using System.Diagnostics;
using Windows.UI.Xaml.Data;

namespace OneDo.Converters
{

    public class ConditionConverter : IValueConverter
    {
        public bool Debug { get; set; }

        public object When { get; set; }

        public object Then { get; set; }

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
                return object.Equals(value, parameter ?? When)
                    ? Then
                    : Otherwise;
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
                return object.Equals(value, Then) ? When : OtherwiseValueBack;
            }
            catch
            {
                return OtherwiseValueBack;
            }
        }
    }
}
