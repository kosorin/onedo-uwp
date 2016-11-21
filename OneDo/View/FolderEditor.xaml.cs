using OneDo.ViewModel;
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

namespace OneDo.View
{
    public sealed partial class FolderEditor : ModalBase, IXBind<FolderEditorViewModel>
    {
        public FolderEditorViewModel VM => ViewModel as FolderEditorViewModel;

        private bool isSelectionChanging = false;

        public FolderEditor()
        {
            InitializeComponent();
        }

        private void Colors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isSelectionChanging)
            {
                isSelectionChanging = true;

                var gridView = (GridView)sender;
                var selectedItem = e.AddedItems.FirstOrDefault();
                if (selectedItem != null)
                {
                    gridView
                        .SelectedItems
                        .Where(x => x != selectedItem)
                        .ToList()
                        .ForEach(x => gridView.SelectedItems.Remove(x));
                }

                isSelectionChanging = false;
            }
        }
    }
}
