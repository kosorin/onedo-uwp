using OneDo.Application.Queries.Folders;
using OneDo.ViewModel;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;
using OneDo.ViewModel.Parameters;

namespace OneDo.View
{
    public sealed partial class FolderEditor : ModalView, IXBind<FolderEditorViewModel>
    {
        public FolderEditorViewModel VM => ViewModel as FolderEditorViewModel;

        private bool isSelectionChanging = false;

        public FolderEditor(FolderEditorParameters parameters) : base(parameters)
        {
            InitializeComponent();
        }

        protected override async Task OnFirstLoad()
        {
            var parameters = (FolderEditorParameters)Parameters;
            await VM.Load(parameters.EntityId);
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
