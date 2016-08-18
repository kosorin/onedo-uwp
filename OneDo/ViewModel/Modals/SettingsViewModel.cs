﻿using OneDo.Model.Data;
using OneDo.Services.ModalService;
using Windows.ApplicationModel;

namespace OneDo.ViewModel.Modals
{
    public class SettingsViewModel : ModalViewModel
    {
        public SettingsViewModel(IModalService modalService, ISettingsProvider settingsProvider) : base(modalService, settingsProvider)
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