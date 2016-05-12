using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OneDo.Controls
{
    [TemplatePart(Name = MenuButtonPartName, Type = typeof(Button))]
    public class HamburgerTitleBar : ContentControl
    {
        public const string MenuButtonPartName = "PART_MenuButton";


        public ICommand MenuButtonCommand
        {
            get { return (ICommand)GetValue(MenuButtonCommandProperty); }
            set { SetValue(MenuButtonCommandProperty, value); }
        }
        public static readonly DependencyProperty MenuButtonCommandProperty =
            DependencyProperty.Register(nameof(MenuButtonCommand), typeof(ICommand), typeof(HamburgerTitleBar), new PropertyMetadata(null));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(HamburgerTitleBar), new PropertyMetadata(null));


        public HamburgerTitleBar()
        {
            DefaultStyleKey = typeof(HamburgerTitleBar);
        }
    }
}
