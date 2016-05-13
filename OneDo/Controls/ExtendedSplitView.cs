using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OneDo.Controls
{
    public class ExtendedSplitView : SplitView
    {
        public bool KeepPaneOpen
        {
            get { return (bool)GetValue(KeepPaneOpenProperty); }
            set { SetValue(KeepPaneOpenProperty, value); }
        }
        public static readonly DependencyProperty KeepPaneOpenProperty =
            DependencyProperty.Register(nameof(KeepPaneOpen), typeof(bool), typeof(ExtendedSplitView), new PropertyMetadata(false));
    }
}
