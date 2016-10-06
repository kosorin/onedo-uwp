using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace OneDo.View.Behaviors
{
    public static class FlyoutMenuItemBehavior
    {
        public static string GetGlyph(DependencyObject obj)
        {
            return (string)obj.GetValue(GlyphProperty);
        }

        public static void SetGlyph(DependencyObject obj, string value)
        {
            obj.SetValue(GlyphProperty, value);
        }

        public static readonly DependencyProperty GlyphProperty =
            DependencyProperty.RegisterAttached("Glyph", typeof(string), typeof(FlyoutMenuItemBehavior), new PropertyMetadata(" "));
    }
}
