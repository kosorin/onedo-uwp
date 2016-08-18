using Microsoft.Xaml.Interactivity;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace OneDo.View.Behaviors
{
    public class ShowAttachedFlyoutAction : DependencyObject, IAction
    {
        public object Execute(object sender, object parameter)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
            return null;
        }
    }
}