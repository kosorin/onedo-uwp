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
    public class UndoBar : Control
    {
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(nameof(Message), typeof(string), typeof(UndoBar), new PropertyMetadata(""));

        public string ButtonGlyph
        {
            get { return (string)GetValue(ButtonGlyphProperty); }
            set { SetValue(ButtonGlyphProperty, value); }
        }

        public static readonly DependencyProperty ButtonGlyphProperty =
            DependencyProperty.Register(nameof(ButtonGlyph), typeof(string), typeof(UndoBar), new PropertyMetadata(null));

        public string ButtonText
        {
            get { return (string)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }

        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register(nameof(ButtonText), typeof(string), typeof(UndoBar), new PropertyMetadata("Undo"));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(UndoBar), new PropertyMetadata(null));

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(UndoBar), new PropertyMetadata(null));

        public UndoBar()
        {
            DefaultStyleKey = typeof(UndoBar);
        }
    }
}
