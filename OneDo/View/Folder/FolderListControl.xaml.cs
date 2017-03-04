using OneDo.View.Core;
using OneDo.ViewModel;
using OneDo.ViewModel.Folder;

namespace OneDo.View.Folder
{
    public sealed partial class FolderListControl : ExtendedUserControl, IView<FolderListViewModel>
    {
        public FolderListViewModel VM => (FolderListViewModel)ViewModel;

        public FolderListControl()
        {
            InitializeComponent();
        }
    }
}
