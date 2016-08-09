using GalaSoft.MvvmLight;
using OneDo.ViewModels;

namespace OneDo.Views
{
    public interface IView
    {
        ExtendedViewModel ViewModel { get; set; }
    }
}