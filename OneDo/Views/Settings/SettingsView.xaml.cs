using OneDo.ViewModels;
using OneDo.ViewModels.Pages;
using OneDo.ViewModels.Settings;

namespace OneDo.Views.Pages
{
    public sealed partial class SettingsView : IXBind<SettingsViewModel>
    {
        public SettingsViewModel VM => ViewModel as SettingsViewModel;

        public SettingsView()
        {
            InitializeComponent();
        }
    }
}
