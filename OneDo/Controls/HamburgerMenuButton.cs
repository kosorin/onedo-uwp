using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace OneDo.Controls
{
    public class HamburgerMenuButton : ToggleButton
    {
        public IconElement Icon
        {
            get { return (IconElement)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(IconElement), typeof(HamburgerMenuButton), new PropertyMetadata(null));

        public Visibility LabelVisibility
        {
            get { return (Visibility)GetValue(LabelVisibilityProperty); }
            set { SetValue(LabelVisibilityProperty, value); }
        }
        public static readonly DependencyProperty LabelVisibilityProperty =
            DependencyProperty.Register(nameof(LabelVisibility), typeof(Visibility), typeof(HamburgerMenuButton), new PropertyMetadata(Visibility.Visible));


        public HamburgerMenuButton()
        {
            DefaultStyleKey = typeof(HamburgerMenuButton);
        }

        /// <summary>
        /// Called when the ToggleButton receives toggle stimulus. Overrided to disable the auto-toggling of the control.
        /// </summary>
        protected override void OnToggle()
        {
        }
    }
}