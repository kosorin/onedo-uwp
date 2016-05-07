using GalaSoft.MvvmLight;
using Windows.UI.Xaml.Controls;

namespace OneDo.View
{
    public abstract class BasePage : Page
    {
        protected ViewModelBase ViewModel { get; set; }

        protected BasePage()
        {
            DataContextChanged += (s, e) => ViewModel = e.NewValue as ViewModelBase;
        }
    }
}
