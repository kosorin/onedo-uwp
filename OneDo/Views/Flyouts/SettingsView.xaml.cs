using OneDo.ViewModels;
using OneDo.ViewModels.Flyouts;
using OneDo.ViewModels.Pages;

namespace OneDo.Views.Flyouts
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
