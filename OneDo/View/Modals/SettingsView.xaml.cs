using OneDo.ViewModel;
using OneDo.ViewModel.Modals;
using OneDo.ViewModel.Pages;

namespace OneDo.View.Modals
{
    public sealed partial class SettingsView : ModalBase, IXBind<SettingsViewModel>
    {
        public SettingsViewModel VM => (SettingsViewModel)ViewModel;

        public SettingsView()
        {
            InitializeComponent();
        }
    }
}
