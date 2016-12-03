using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OneDo.View.Behaviors
{
    public static class TextBoxBehavior
    {
        public static bool GetFocusOnLoad(DependencyObject obj)
        {
            return (bool)obj.GetValue(FocusOnLoadProperty);
        }

        public static void SetFocusOnLoad(DependencyObject obj, bool value)
        {
            obj.SetValue(FocusOnLoadProperty, value);
        }

        public static readonly DependencyProperty FocusOnLoadProperty =
            DependencyProperty.RegisterAttached("FocusOnLoad", typeof(bool), typeof(TextBoxBehavior), new PropertyMetadata(false, FocusOnLoad_Changed));

        private static void FocusOnLoad_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as TextBox;
            if (element != null)
            {
                element.Loaded -= Element_Loaded;
                if ((bool)e.NewValue)
                {
                    element.Loaded += Element_Loaded;
                }
            }
        }

        private static void Element_Loaded(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Focus(FocusState.Programmatic);
        }
    }
}
