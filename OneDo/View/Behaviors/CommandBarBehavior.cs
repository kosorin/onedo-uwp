﻿using OneDo.Common.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace OneDo.View.Behaviors
{
    public static class CommandBarBehavior
    {
        public static bool GetHideMoreButton(DependencyObject obj)
        {
            return (bool)obj.GetValue(HideMoreButtonProperty);
        }

        public static void SetHideMoreButton(DependencyObject obj, bool value)
        {
            obj.SetValue(HideMoreButtonProperty, value);
        }

        public static readonly DependencyProperty HideMoreButtonProperty =
            DependencyProperty.RegisterAttached("HideMoreButton", typeof(bool), typeof(CommandBarBehavior), new PropertyMetadata(false, HideMoreButton_Changed));

        private static void HideMoreButton_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var commandBar = d as CommandBar;
            var morebutton = TreeHelper.FindChild<ButtonBase>(commandBar, "MoreButton");
            if (morebutton != null)
            {
                var value = GetHideMoreButton(commandBar);
                morebutton.Visibility = (bool)e.NewValue ? Visibility.Collapsed : Visibility.Visible;
            }
            else
            {
                commandBar.Loaded += CommandBar_Loaded;
            }
        }

        private static void CommandBar_Loaded(object sender, RoutedEventArgs e)
        {
            var commandBar = sender as CommandBar;
            var morebutton = TreeHelper.FindChild<ButtonBase>(commandBar, "MoreButton");
            if (morebutton != null)
            {
                var value = GetHideMoreButton(commandBar);
                morebutton.Visibility = value ? Visibility.Collapsed : Visibility.Visible;
                commandBar.Loaded -= CommandBar_Loaded;
            }
        }
    }
}
