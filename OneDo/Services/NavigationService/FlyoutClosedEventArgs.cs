using OneDo.ViewModels;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace OneDo.Services.NavigationService
{
    public class FlyoutClosedEventArgs : EventArgs
    {
        public FlyoutViewModel ViewModel { get; set; }

        public FlyoutCloseType CloseType { get; set; }
    }
}