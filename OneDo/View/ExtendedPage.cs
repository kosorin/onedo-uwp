using GalaSoft.MvvmLight;
using OneDo.ViewModel;
using System.ComponentModel;
using Windows.ApplicationModel;
using Windows.UI.Composition;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace OneDo.View
{
    public class ExtendedPage : Page, IView, INotifyPropertyChanged
    {
        public ExtendedViewModel ViewModel { get; set; }

        protected readonly Compositor compositor;

        public ExtendedPage()
        {
            if (!DesignMode.DesignModeEnabled)
            {
                DataContextChanged += (s, e) =>
                {
                    ViewModel = e.NewValue as ExtendedViewModel;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IXBind<ExtendedViewModel>.VM)));
                };
            }

            compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
