using OneDo.ViewModel;

namespace OneDo.View
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
