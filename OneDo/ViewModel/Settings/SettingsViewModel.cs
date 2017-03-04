using OneDo.Common.Meta;
using OneDo.ViewModel.Core;

namespace OneDo.ViewModel.Settings
{
    public class SettingsViewModel : ModalViewModel
    {
        public string VersionText => $"Version {AppInformation.GetVersion()}";
    }
}