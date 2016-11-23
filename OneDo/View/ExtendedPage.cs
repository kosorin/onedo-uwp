using OneDo.ViewModel;
using System.ComponentModel;
using Windows.ApplicationModel;
using Windows.UI.Composition;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

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
                    OnViewModelChanging();

                    ViewModel = e.NewValue as ExtendedViewModel;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IXBind<ExtendedViewModel>.VM)));

                    OnViewModelChanged();
                };
            }

            compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
        }

        protected virtual void OnViewModelChanged()
        {

        }

        protected virtual void OnViewModelChanging()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
