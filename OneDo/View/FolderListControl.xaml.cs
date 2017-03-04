using OneDo.Common.Mvvm;
using OneDo.ViewModel;

namespace OneDo.View
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
