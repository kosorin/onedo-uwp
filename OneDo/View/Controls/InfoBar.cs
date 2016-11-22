using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OneDo.View.Controls
{
    public class InfoBar : Control
    {
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(nameof(Message), typeof(string), typeof(InfoBar), new PropertyMetadata(""));

        public Visibility ActionVisibility
        {
            get { return (Visibility)GetValue(ActionVisibilityProperty); }
            set { SetValue(ActionVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ActionVisibilityProperty =
            DependencyProperty.Register(nameof(ActionVisibility), typeof(Visibility), typeof(InfoBar), new PropertyMetadata(Visibility.Visible));

        public string ActionGlyph
        {
            get { return (string)GetValue(ActionGlyphProperty); }
            set { SetValue(ActionGlyphProperty, value); }
        }

        public static readonly DependencyProperty ActionGlyphProperty =
            DependencyProperty.Register(nameof(ActionGlyph), typeof(string), typeof(InfoBar), new PropertyMetadata(null));

        public string ActionText
        {
            get { return (string)GetValue(ActionTextProperty); }
            set { SetValue(ActionTextProperty, value); }
        }

        public static readonly DependencyProperty ActionTextProperty =
            DependencyProperty.Register(nameof(ActionText), typeof(string), typeof(InfoBar), new PropertyMetadata("Undo"));

        public ICommand ActionCommand
        {
            get { return (ICommand)GetValue(ActionCommandProperty); }
            set { SetValue(ActionCommandProperty, value); }
        }

        public static readonly DependencyProperty ActionCommandProperty =
            DependencyProperty.Register(nameof(ActionCommand), typeof(ICommand), typeof(InfoBar), new PropertyMetadata(null));

        public object ActionCommandParameter
        {
            get { return (object)GetValue(ActionCommandParameterProperty); }
            set { SetValue(ActionCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty ActionCommandParameterProperty =
            DependencyProperty.Register(nameof(ActionCommandParameter), typeof(object), typeof(InfoBar), new PropertyMetadata(null));

        public InfoBar()
        {
            DefaultStyleKey = typeof(InfoBar);
        }
    }
}
