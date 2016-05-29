using GalaSoft.MvvmLight;
using OneDo.Common.Logging;
using OneDo.Model.Data;
using OneDo.Services.NavigationService;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace OneDo.ViewModels
{
    public abstract class PageViewModel : ExtendedViewModelBase, INavigable
    {
        public INavigationService NavigationService { get; }

        public IDataProvider DataProvider { get; }

        protected PageViewModel(INavigationService navigationService, IDataProvider dataProvider)
        {
            NavigationService = navigationService;
            DataProvider = dataProvider;
        }


        public virtual void OnNavigatingFrom(NavigatingEventArgs args)
        {

        }

        public virtual Task OnNavigatedFromAsync()
        {
            return Task.CompletedTask;
        }

        public virtual void OnNavigatedTo(object parameter, NavigationMode mode)
        {

        }

        public virtual void OnBackButton(BackButtonEventArgs args)
        {

        }
    }
}
