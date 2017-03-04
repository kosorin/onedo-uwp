using OneDo.Common.Extensions;
using OneDo.ViewModel.Core;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace OneDo.View.Core
{
    public class ExtendedUserControl : UserControl, IView, INotifyPropertyChanged
    {
        public ExtendedViewModel ViewModel { get; set; }

        protected readonly Compositor compositor;

        public ExtendedUserControl()
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
