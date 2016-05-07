using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using OneDo.ViewModel.Pages;

namespace OneDo.ViewModel
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //if (ViewModelBase.IsInDesignModeStatic)
            //{
            //    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            //}
            //else
            //{
            //    SimpleIoc.Default.Register<IDataService, DataService>();
            //}

            //SimpleIoc.Default.Register<INavigationService, ShellNavigationService>();

            SimpleIoc.Default.Register<ShellViewModel>();

            SimpleIoc.Default.Register<MainPageViewModel>();
        }

        public ShellViewModel Shell => ServiceLocator.Current.GetInstance<ShellViewModel>();

        public MainPageViewModel MainPage => ServiceLocator.Current.GetInstance<MainPageViewModel>();
    }
}
