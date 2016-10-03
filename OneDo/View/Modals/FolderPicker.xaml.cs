using OneDo.ViewModel.Items;
using OneDo.ViewModel.Modals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public sealed partial class FolderPicker : ExtendedUserControl, IXBind<FolderPickerViewModel>
    {
        public FolderPickerViewModel VM => (FolderPickerViewModel)ViewModel;

        public FolderPicker()
        {
            InitializeComponent();
        }

        private async void Folder_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            var folder = element?.DataContext as FolderItemObject;
            if (folder != null)
            {
                await VM.Pick(folder);
            }
        }
    }
}
