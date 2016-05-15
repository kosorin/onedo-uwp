using GalaSoft.MvvmLight;
using Windows.UI.Xaml.Controls;

namespace OneDo.Views
{
    public abstract class PageBase : Page
    {
        protected ViewModelBase ViewModel { get; set; }

        protected PageBase()
        {
            DataContextChanged += (s, e) =>
            {
                ViewModel = e.NewValue as ViewModelBase;
            };
        }
    }
}
