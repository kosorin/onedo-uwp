using OneDo.Application.Queries.Folders;
using OneDo.ViewModel;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;
using OneDo.ViewModel.Folder;
using OneDo.View.Core;

namespace OneDo.View.Folder
{
    public sealed partial class FolderEditorView : ModalView, IView<FolderEditorViewModel>
    {
        public FolderEditorViewModel VM => ViewModel as FolderEditorViewModel;

        public Guid? FolderId { get; }

        private bool isSelectionChanging = false;

        public FolderEditorView(Guid? folderId)
        {
            InitializeComponent();

            FolderId = folderId;
        }

        protected override async Task OnFirstLoad()
        {
            if (FolderId == null)
            {
                NameTextBox.Focus(FocusState.Programmatic);
            }
            await VM.Load(FolderId);
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
