using OneDo.Common.UI;
using OneDo.ViewModel.Modals;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace OneDo.View.Modals
{
    public sealed partial class DebugView : ModalBase, IXBind<DebugViewModel>
    {
        public DebugViewModel VM => (DebugViewModel)ViewModel;

        public DebugView()
        {
            InitializeComponent();
        }

        private void Log_TextChanged(object sender, TextChangedEventArgs e)
        {
            var scrollViewer = TreeHelper.FindChild<ScrollViewer>((DependencyObject)sender);
            if (scrollViewer != null)
            {
                scrollViewer.ChangeView(0, scrollViewer.ExtentHeight, 1, true);
            }
        }

        private async void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Contains(LogPivotItem))
            {
                await VM.LoadLog();
            }
        }
    }
}
