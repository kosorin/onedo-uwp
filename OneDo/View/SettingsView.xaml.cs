using OneDo.Common.Mvvm;
using OneDo.ViewModel;

namespace OneDo.View
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
