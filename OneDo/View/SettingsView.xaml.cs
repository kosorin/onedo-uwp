using OneDo.ViewModel;

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
