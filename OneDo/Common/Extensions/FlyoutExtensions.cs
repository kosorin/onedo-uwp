using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace OneDo.Common.Extensions
{
    public static class FlyoutExtensions
    {
        public static void ShowAttachedMenuFlyout(this FrameworkElement element, HoldingRoutedEventArgs args)
        {
            if (args.HoldingState == HoldingState.Started)
            {
                var flyout = FlyoutBase.GetAttachedFlyout(element) as MenuFlyout;
                if (flyout != null)
                {
                    flyout.ShowAt(element, args.GetPosition(element));
                }
                args.Handled = true;
            }
        }

        public static void ShowAttachedMenuFlyout(this FrameworkElement element, RightTappedRoutedEventArgs args)
        {
            var flyout = FlyoutBase.GetAttachedFlyout(element) as MenuFlyout;
            if (flyout != null)
            {
                flyout.ShowAt(element, args.GetPosition(element));
            }
            args.Handled = true;
        }
    }
}
