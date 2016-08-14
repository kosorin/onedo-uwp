﻿using OneDo.ViewModel.Controls;
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
using SystemFlyoutBase = Windows.UI.Xaml.Controls.Primitives.FlyoutBase;

namespace OneDo.View.Controls
{
    public sealed partial class FolderListControl : ExtendedUserControl, IXBind<FolderListViewModel>
    {
        public FolderListViewModel VM => (FolderListViewModel)ViewModel;

        public FolderListControl()
        {
            InitializeComponent();
        }

        private void FolderItem_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            SystemFlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }
    }
}
