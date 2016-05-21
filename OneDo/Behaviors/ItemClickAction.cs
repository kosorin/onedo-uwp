using Microsoft.Xaml.Interactivity;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OneDo.Behaviors
{
    public class ItemClickAction : DependencyObject, IAction
    {
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(ItemClickAction), new PropertyMetadata(null));


        public object Execute(object sender, object parameter)
        {
            var clickedItem = (parameter as ItemClickEventArgs)?.ClickedItem;
            if (clickedItem != null)
            {
                Command?.Execute(clickedItem);
            }
            return null;
        }
    }
}
