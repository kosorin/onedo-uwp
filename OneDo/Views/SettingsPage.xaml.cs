using OneDo.ViewModels;

namespace OneDo.Views
{
    public sealed partial class SettingsPage : IXBindPage<SettingsViewModel>
    {
        public SettingsViewModel VM => ViewModel as SettingsViewModel;

        public SettingsPage()
        {
            InitializeComponent();
        }
    }
}
