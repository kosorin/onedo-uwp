﻿using GalaSoft.MvvmLight;
using OneDo.Services.DataService;
using OneDo.Services.NavigationService;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Windows.ApplicationModel;

namespace OneDo.ViewModel.Pages
{
    public class AboutPageViewModel : BasePageViewModel
    {
        public string VersionText => $"Version {GetAppVersion()}";

        public AboutPageViewModel(ShellViewModel shell, INavigationService navigationService)
            : base(shell, navigationService)
        {

        }

        public static string GetAppVersion()
        {
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }

        public override void OnNavigatedTo(object parameter, NavigationMode mode)
        {
            Shell.Title = "About";
        }
    }
}