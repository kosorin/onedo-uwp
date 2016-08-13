using OneDo.ViewModels.Items;
using OneDo.Views.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace OneDo.Controls
{
    public class FolderListView : ListView
    {
        public FolderListView()
        {
            DefaultStyleKey = nameof(FolderListView);

            SelectionChanged += OnSelectionChanged;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selector = ItemTemplateSelector as FolderTemplateSelector;
            if (selector != null)
            {
                selector.SelectedFolder = (FolderItemViewModel)e.AddedItems.FirstOrDefault();
                ItemTemplateSelector = null;
                ItemTemplateSelector = selector;
            }
        }
    }
}
