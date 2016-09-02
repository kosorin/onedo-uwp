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
    }
}
