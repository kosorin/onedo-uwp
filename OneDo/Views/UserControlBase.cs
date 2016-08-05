using GalaSoft.MvvmLight;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace OneDo.Views
{
    public abstract class UserControlBase : UserControl, IView, INotifyPropertyChanged
    {
        public ViewModelBase ViewModel { get; set; }

        protected UserControlBase()
        {
            DataContextChanged += (s, e) =>
            {
                ViewModel = e.NewValue as ViewModelBase;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VM"));
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}