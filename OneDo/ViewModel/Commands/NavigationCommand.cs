﻿using GalaSoft.MvvmLight.Command;
using OneDo.Services.NavigationService;
using OneDo.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OneDo.ViewModel.Commands
{
    public class NavigationCommand<TBasePage> : ICommand where TBasePage : BasePage
    {
        public event EventHandler CanExecuteChanged;

        public INavigationService NavigationService { get; }

        public NavigationCommand(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            NavigationService.Navigate<TBasePage>();
        }
    }
}