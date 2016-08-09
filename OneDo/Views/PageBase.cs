using GalaSoft.MvvmLight;
using OneDo.ViewModels;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace OneDo.Views
{
    public class PageBase : Page, IView, INotifyPropertyChanged
    {
        public ViewModelBase ViewModel { get; set; }

        public PageBase()
        {
            if (!ViewModelBase.IsInDesignModeStatic)
            {
                DataContextChanged += (s, e) =>
                {
                    ViewModel = e.NewValue as ViewModelBase;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VM"));
                };
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
