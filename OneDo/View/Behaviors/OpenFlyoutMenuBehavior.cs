using Microsoft.Xaml.Interactivity;
using OneDo.Common.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace OneDo.View.Behaviors
{
    public class OpenFlyoutMenuBehavior : DependencyObject, IBehavior
    {
        public DependencyObject AssociatedObject { get; set; }

        public void Attach(DependencyObject associatedObject)
        {
            if (associatedObject != AssociatedObject && !DesignMode.DesignModeEnabled)
            {
                AssociatedObject = associatedObject;
                var element = AssociatedObject as FrameworkElement;
                if (element != null)
                {
                    element.Holding += Holding;
                    element.RightTapped += RightTapped;
                }
            }
        }

        public void Detach()
        {
            var element = AssociatedObject as FrameworkElement;
            if (element != null)
            {
                element.Holding += Holding;
                element.RightTapped += RightTapped;
            }
            AssociatedObject = null;
        }

        private void Holding(object sender, HoldingRoutedEventArgs e)
        {
            var element = AssociatedObject as FrameworkElement;
            if (element != null)
            {
                element.RightTapped -= RightTapped;
                FlyoutExtensions.ShowAttachedMenuFlyout(element, e);
            }
        }

        private void RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var element = AssociatedObject as FrameworkElement;
            if (element != null)
            {
                FlyoutExtensions.ShowAttachedMenuFlyout(element, e);
            }
        }
    }
}
