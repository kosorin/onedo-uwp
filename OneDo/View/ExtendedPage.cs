using OneDo.Common.Mvvm;
using OneDo.Common.Extensions;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
                    if (!ReferenceEquals(ViewModel, e.NewValue))
                    {
                        OnViewModelChanging();

                        ViewModel = e.NewValue as ExtendedViewModel;
                        if (GetType().ImplementsGenericInterface(typeof(IXBind<>)))
                        {
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IXBind<ExtendedViewModel>.VM)));
                        }

                        OnViewModelChanged();
                    }
                };
            }

            compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
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
