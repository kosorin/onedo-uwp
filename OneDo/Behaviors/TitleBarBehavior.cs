using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace OneDo.Behaviors
{
    public static class TitleBarBehavior
    {
        public static ApplicationViewTitleBar Current => ApplicationView.GetForCurrentView().TitleBar;


        public static Color GetBackgroundColor(DependencyObject d)
        {
            return (Color)d.GetValue(BackgroundColorProperty);
        }
        public static void SetBackgroundColor(DependencyObject d, Color value)
        {
            d.SetValue(BackgroundColorProperty, value);
        }
        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.RegisterAttached("BackgroundColor", typeof(Color), typeof(TitleBarBehavior),
                new PropertyMetadata(null, (d, e) => Current.BackgroundColor = (Color)e.NewValue));

        public static Color GetForegroundColor(DependencyObject d)
        {
            return (Color)d.GetValue(ForegroundColorProperty);
        }
        public static void SetForegroundColor(DependencyObject d, Color value)
        {
            d.SetValue(ForegroundColorProperty, value);
        }
        public static readonly DependencyProperty ForegroundColorProperty =
            DependencyProperty.RegisterAttached("ForegroundColor", typeof(Color), typeof(TitleBarBehavior),
                new PropertyMetadata(null, (d, e) => Current.ForegroundColor = (Color)e.NewValue));

        public static Color GetButtonBackgroundColor(DependencyObject d)
        {
            return (Color)d.GetValue(ButtonBackgroundColorProperty);
        }
        public static void SetButtonBackgroundColor(DependencyObject d, Color value)
        {
            d.SetValue(ButtonBackgroundColorProperty, value);
        }
        public static readonly DependencyProperty ButtonBackgroundColorProperty =
            DependencyProperty.RegisterAttached("ButtonBackgroundColor", typeof(Color), typeof(TitleBarBehavior),
                new PropertyMetadata(null, (d, e) => Current.ButtonBackgroundColor = (Color)e.NewValue));

        public static Color GetButtonForegroundColor(DependencyObject d)
        {
            return (Color)d.GetValue(ButtonForegroundColorProperty);
        }
        public static void SetButtonForegroundColor(DependencyObject d, Color value)
        {
            d.SetValue(ButtonForegroundColorProperty, value);
        }
        public static readonly DependencyProperty ButtonForegroundColorProperty =
            DependencyProperty.RegisterAttached("ButtonForegroundColor", typeof(Color), typeof(TitleBarBehavior),
                new PropertyMetadata(null, (d, e) => Current.ButtonForegroundColor = (Color)e.NewValue));

        public static Color GetButtonHoverBackgroundColor(DependencyObject d)
        {
            return (Color)d.GetValue(ButtonHoverBackgroundColorProperty);
        }
        public static void SetButtonHoverBackgroundColor(DependencyObject d, Color value)
        {
            d.SetValue(ButtonHoverBackgroundColorProperty, value);
        }
        public static readonly DependencyProperty ButtonHoverBackgroundColorProperty =
            DependencyProperty.RegisterAttached("ButtonHoverBackgroundColor", typeof(Color), typeof(TitleBarBehavior),
                new PropertyMetadata(null, (d, e) => Current.ButtonHoverBackgroundColor = (Color)e.NewValue));

        public static Color GetButtonHoverForegroundColor(DependencyObject d)
        {
            return (Color)d.GetValue(ButtonHoverForegroundColorProperty);
        }
        public static void SetButtonHoverForegroundColor(DependencyObject d, Color value)
        {
            d.SetValue(ButtonHoverForegroundColorProperty, value);
        }
        public static readonly DependencyProperty ButtonHoverForegroundColorProperty =
            DependencyProperty.RegisterAttached("ButtonHoverForegroundColor", typeof(Color), typeof(TitleBarBehavior),
                new PropertyMetadata(null, (d, e) => Current.ButtonHoverForegroundColor = (Color)e.NewValue));

        public static Color GetButtonInactiveBackgroundColor(DependencyObject d)
        {
            return (Color)d.GetValue(ButtonInactiveBackgroundColorProperty);
        }
        public static void SetButtonInactiveBackgroundColor(DependencyObject d, Color value)
        {
            d.SetValue(ButtonInactiveBackgroundColorProperty, value);
        }
        public static readonly DependencyProperty ButtonInactiveBackgroundColorProperty =
            DependencyProperty.RegisterAttached("ButtonInactiveBackgroundColor", typeof(Color), typeof(TitleBarBehavior),
                new PropertyMetadata(null, (d, e) => Current.ButtonInactiveBackgroundColor = (Color)e.NewValue));

        public static Color GetButtonInactiveForegroundColor(DependencyObject d)
        {
            return (Color)d.GetValue(ButtonInactiveForegroundColorProperty);
        }
        public static void SetButtonInactiveForegroundColor(DependencyObject d, Color value)
        {
            d.SetValue(ButtonInactiveForegroundColorProperty, value);
        }
        public static readonly DependencyProperty ButtonInactiveForegroundColorProperty =
            DependencyProperty.RegisterAttached("ButtonInactiveForegroundColor", typeof(Color), typeof(TitleBarBehavior),
                new PropertyMetadata(null, (d, e) => Current.ButtonInactiveForegroundColor = (Color)e.NewValue));

        public static Color GetButtonPressedBackgroundColor(DependencyObject d)
        {
            return (Color)d.GetValue(ButtonPressedBackgroundColorProperty);
        }
        public static void SetButtonPressedBackgroundColor(DependencyObject d, Color value)
        {
            d.SetValue(ButtonPressedBackgroundColorProperty, value);
        }
        public static readonly DependencyProperty ButtonPressedBackgroundColorProperty =
            DependencyProperty.RegisterAttached("ButtonPressedBackgroundColor", typeof(Color), typeof(TitleBarBehavior),
                new PropertyMetadata(null, (d, e) => Current.ButtonPressedBackgroundColor = (Color)e.NewValue));

        public static Color GetButtonPressedForegroundColor(DependencyObject d)
        {
            return (Color)d.GetValue(ButtonPressedForegroundColorProperty);
        }
        public static void SetButtonPressedForegroundColor(DependencyObject d, Color value)
        {
            d.SetValue(ButtonPressedForegroundColorProperty, value);
        }
        public static readonly DependencyProperty ButtonPressedForegroundColorProperty =
            DependencyProperty.RegisterAttached("ButtonPressedForegroundColor", typeof(Color), typeof(TitleBarBehavior),
                new PropertyMetadata(null, (d, e) => Current.ButtonPressedForegroundColor = (Color)e.NewValue));

        public static Color GetInactiveBackgroundColor(DependencyObject d)
        {
            return (Color)d.GetValue(InactiveBackgroundColorProperty);
        }
        public static void SetInactiveBackgroundColor(DependencyObject d, Color value)
        {
            d.SetValue(InactiveBackgroundColorProperty, value);
        }
        public static readonly DependencyProperty InactiveBackgroundColorProperty =
            DependencyProperty.RegisterAttached("InactiveBackgroundColor", typeof(Color), typeof(TitleBarBehavior),
                new PropertyMetadata(null, (d, e) => Current.InactiveBackgroundColor = (Color)e.NewValue));

        public static Color GetInactiveForegroundColor(DependencyObject d)
        {
            return (Color)d.GetValue(InactiveForegroundColorProperty);
        }
        public static void SetInactiveForegroundColor(DependencyObject d, Color value)
        {
            d.SetValue(InactiveForegroundColorProperty, value);
        }
        public static readonly DependencyProperty InactiveForegroundColorProperty =
            DependencyProperty.RegisterAttached("InactiveForegroundColor", typeof(Color), typeof(TitleBarBehavior),
                new PropertyMetadata(null, (d, e) => Current.InactiveForegroundColor = (Color)e.NewValue));
    }
}
