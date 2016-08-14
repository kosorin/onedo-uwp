using OneDo.ViewModel;
using OneDo.ViewModel.Flyouts;
using OneDo.ViewModel.Pages;

namespace OneDo.View.Flyouts
{
    public sealed partial class SettingsView : FlyoutBase, IXBind<SettingsViewModel>
    {
        public SettingsViewModel VM => (SettingsViewModel)ViewModel;

        public SettingsView()
        {
            InitializeComponent();
        }
    }
}
