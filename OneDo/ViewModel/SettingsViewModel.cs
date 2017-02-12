using OneDo.Common.Meta;

namespace OneDo.ViewModel
{
    public class SettingsViewModel : ModalViewModel
    {
        public string VersionText => $"Version {AppInformation.GetVersion()}";
    }
}