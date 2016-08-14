using GalaSoft.MvvmLight;
using OneDo.ViewModel;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace OneDo.View
{
    public class ExtendedPage : Page, IView, INotifyPropertyChanged
    {
        public ExtendedViewModel ViewModel { get; set; }

        public ExtendedPage()
        {
            if (!ViewModelBase.IsInDesignModeStatic)
            {
                DataContextChanged += (s, e) =>
                {
                    ViewModel = e.NewValue as ExtendedViewModel;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VM"));
                };
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
