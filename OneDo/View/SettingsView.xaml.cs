using OneDo.ViewModel;

namespace OneDo.View
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
