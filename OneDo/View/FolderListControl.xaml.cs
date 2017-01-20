using OneDo.ViewModel;

namespace OneDo.View
{
    public sealed partial class FolderListControl : ExtendedUserControl, IXBind<FolderListViewModel>
    {
        public FolderListViewModel VM => (FolderListViewModel)ViewModel;

        public FolderListControl()
        {
            InitializeComponent();
        }

        protected override async void OnViewModelChanged()
        {
            await VM?.Load();
        }
    }
}
