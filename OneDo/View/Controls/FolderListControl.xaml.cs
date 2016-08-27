using OneDo.ViewModel;
using OneDo.ViewModel.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace OneDo.View.Controls
{
    public sealed partial class FolderListControl : ExtendedUserControl, IXBind<FolderListViewModel>
    {
        public FolderListViewModel VM => (FolderListViewModel)ViewModel;

        public FolderListControl()
        {
            InitializeComponent();
        }

        private void AddItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            VM.AddItem();
        }

        private void EditItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var item = (FolderItemObject)element.DataContext;
            VM.EditItem(item);
        }

        private async void DeleteItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var item = (FolderItemObject)element.DataContext;
            await VM.DeleteItem(item);
        }
    }
}
