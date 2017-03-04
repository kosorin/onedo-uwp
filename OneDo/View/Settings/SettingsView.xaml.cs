using OneDo.View.Core;
using OneDo.ViewModel;
using OneDo.ViewModel.Settings;

namespace OneDo.View.Settings
{
    public sealed partial class SettingsView : ModalView, IView<SettingsViewModel>
    {
        public SettingsViewModel VM => (SettingsViewModel)ViewModel;

        public SettingsView()
        {
            InitializeComponent();
        }
    }
}
