﻿using OneDo.ViewModel;
using System.Linq;
using Windows.UI.Xaml.Controls;

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
