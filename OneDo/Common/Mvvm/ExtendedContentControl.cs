using OneDo.Common.Extensions;
using OneDo.Common.Mvvm;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace OneDo.Common.Mvvm
{
    public class ExtendedContentControl : ContentControl, IView, INotifyPropertyChanged
    {
        public ExtendedViewModel ViewModel { get; set; }

        protected readonly Compositor compositor;

        public ExtendedContentControl()
        {
            if (!DesignMode.DesignModeEnabled)
            {
                DataContextChanged += (s, e) =>
                {
                    if (!ReferenceEquals(ViewModel, e.NewValue))
                    {
                        OnViewModelChanging();

                        ViewModel = e.NewValue as ExtendedViewModel;
                        if (GetType().ImplementsGenericInterface(typeof(IView<>)))
                        {
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IView<ExtendedViewModel>.VM)));
                        }

                        OnViewModelChanged();
                    }
                };
            }

            compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;

            Loaded += OnModalLoaded;
        }

        private void OnModalLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnModalLoaded;
            OnFirstLoad();
        }

        protected virtual Task OnFirstLoad()
        {
            return Task.CompletedTask;
        }

        protected virtual void OnViewModelChanging()
        {

        }

        protected virtual void OnViewModelChanged()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}