using GalaSoft.MvvmLight;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace OneDo.Views
{
    public class PageBase : Page, IView
    {
        public ViewModelBase ViewModel { get; set; }

        protected PageBase()
        {
            DataContextChanged += (s, e) =>
            {
                ViewModel = e.NewValue as ViewModelBase;
            };
        }
    }
}
