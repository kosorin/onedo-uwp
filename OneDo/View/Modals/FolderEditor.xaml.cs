using OneDo.ViewModel.Items;
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
    public sealed partial class FolderEditor : ModalBase, IXBind<FolderEditorViewModel>
    {
        public FolderEditorViewModel VM => ViewModel as FolderEditorViewModel;

        public FolderEditor()
        {
            InitializeComponent();
        }

        private void Colors_Loaded(object sender, RoutedEventArgs e)
        {
            var gridView = (GridView)sender;

            foreach (var item in gridView.Items)
            {
                var container = gridView.ContainerFromItem(item) as GridViewItem;
                var isSelected = gridView.SelectedItems.Contains(item);
                SetColorIconVisibility(container, isSelected ? Visibility.Visible : Visibility.Collapsed);
            }
        }

        private void Colors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var gridView = (GridView)sender;

            foreach (var item in e.AddedItems)
            {
                var container = gridView.ContainerFromItem(item) as GridViewItem;
                SetColorIconVisibility(container, Visibility.Visible);
            }
            foreach (var item in e.RemovedItems)
            {
                var container = gridView.ContainerFromItem(item) as GridViewItem;
                SetColorIconVisibility(container, Visibility.Collapsed);
            }
        }

        private void SetColorIconVisibility(GridViewItem container, Visibility visibility)
        {
            if (container != null)
            {
                var root = (FrameworkElement)container.ContentTemplateRoot;
                var icon = root.FindName("SelectedIcon") as FrameworkElement;
                if (icon != null)
                {
                    icon.Visibility = visibility;
                }
            }
        }
    }
}
