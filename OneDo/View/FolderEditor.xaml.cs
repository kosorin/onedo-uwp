using OneDo.Application.Queries.Folders;
using OneDo.ViewModel;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace OneDo.View
{
    public sealed partial class FolderEditor : ModalView, IXBind<FolderEditorViewModel>
    {
        public FolderEditorViewModel VM => ViewModel as FolderEditorViewModel;

        public Guid? FolderId { get; }

        private bool isSelectionChanging = false;

        public FolderEditor(Guid? folderId)
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
