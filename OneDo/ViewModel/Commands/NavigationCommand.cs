using GalaSoft.MvvmLight.Command;
using OneDo.Common.MVVM;
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
    public class NavigationCommand<TBasePage> : BaseCommand where TBasePage : BasePage
    {
        public INavigationService NavigationService { get; }

        public NavigationCommand(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public override void Execute(object parameter)
        {
            NavigationService.Navigate<TBasePage>();
        }
    }
}
