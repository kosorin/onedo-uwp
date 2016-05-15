using OneDo.Services.NavigationService;
using Windows.ApplicationModel;

namespace OneDo.ViewModels
{
    public class AboutViewModel : PageViewModel
    {
        public string VersionText => $"Version {GetAppVersion()}";

        public AboutViewModel(INavigationService navigationService)
            : base(navigationService)
        {

        }

        private static string GetAppVersion()
        {
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }
    }
}