﻿using OneDo.Common.Event;
using OneDo.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OneDo.Services.NavigationService
{
    public interface INavigable
    {
        void OnNavigatingFrom(NavigatingEventArgs args);

        Task OnNavigatedFromAsync();

        void OnNavigatedTo(object parameter, NavigationMode mode);

        void OnBackButton(BackButtonEventArgs args);
    }
}