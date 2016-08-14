using System;
using System.Diagnostics;
using Windows.UI.Xaml.Data;

namespace OneDo.View.Converters
{

    public class ConditionConverter : IValueConverter
    {
        public bool Debug { get; set; }

        public object If { get; set; }

        public object Then { get; set; }

        public object Else { get; set; }

        public object FallbackValue { get; set; }


        public object Convert(object value, Type targetType, object parameter, string language)
        {
#if DEBUG
            if (Debug)
            {
                Debugger.Break();
            }
#endif

            try
            {
                return object.Equals(value, parameter ?? If)
                    ? Then
                    : Else;
            }
            catch
            {
                return Else;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
#if DEBUG
            if (Debug)
            {
                Debugger.Break();
            }
#endif

            if (FallbackValue == null)
            {
                throw new InvalidOperationException($"Cannot {nameof(ConvertBack)} if no {nameof(FallbackValue)} is set!");
            }
            try
            {
                return object.Equals(value, Then) ? If : FallbackValue;
            }
            catch
            {
                return FallbackValue;
            }
        }
    }
}
