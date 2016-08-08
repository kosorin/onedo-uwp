﻿using OneDo.Model.Data;
using OneDo.Services.Context;
using OneDo.Services.NavigationService;
using Windows.ApplicationModel;

namespace OneDo.ViewModels.Flyouts
{
    public class SettingsViewModel : FlyoutViewModel
    {
        public SettingsViewModel(INavigationService navigationService, IDataProvider dataProvider, IContext context) : base(navigationService, dataProvider, context)
        {

        }

        public string VersionText => $"Version {GetAppVersion()}";

        public string GetAppVersion()
        {
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }
    }
}