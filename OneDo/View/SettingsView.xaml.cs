using OneDo.ViewModel;
using OneDo.ViewModel.Parameters;

namespace OneDo.View
{
    public sealed partial class SettingsView : ModalView, IXBind<SettingsViewModel>
    {
        public SettingsViewModel VM => (SettingsViewModel)ViewModel;

        public SettingsView()
        {
            InitializeComponent();
        }
    }
}
