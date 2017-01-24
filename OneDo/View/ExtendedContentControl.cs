using OneDo.Common.Extensions;
using OneDo.Common.Mvvm;
using System.ComponentModel;
using Windows.ApplicationModel;
using Windows.UI.Composition;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace OneDo.View
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