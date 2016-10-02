using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OneDo.View.Controls
{
    public class MenuFlyoutIconItem : MenuFlyoutItem
    {
        public MenuFlyoutIconItem()
        {
            DefaultStyleKey = typeof(MenuFlyoutIconItem);
        }

        public string Glyph
        {
            get { return (string)GetValue(GlyphProperty); }
            set { SetValue(GlyphProperty, value); }
        }

        public static readonly DependencyProperty GlyphProperty =
            DependencyProperty.Register(nameof(Glyph), typeof(string), typeof(MenuFlyoutIconItem), new PropertyMetadata(" "));
    }
}
