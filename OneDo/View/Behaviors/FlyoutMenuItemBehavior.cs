using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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



        public static object GetSubItemContext(DependencyObject obj)
        {
            return (object)obj.GetValue(SubItemContextProperty);
        }

        public static void SetSubItemContext(DependencyObject obj, object value)
        {
            obj.SetValue(SubItemContextProperty, value);
        }

        public static readonly DependencyProperty SubItemContextProperty =
            DependencyProperty.RegisterAttached("SubItemContext", typeof(object), typeof(FlyoutMenuItemBehavior), new PropertyMetadata(null));
    }
}
