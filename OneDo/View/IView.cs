using GalaSoft.MvvmLight;
using OneDo.ViewModel;

namespace OneDo.View
{
    public interface IView
    {
        ExtendedViewModel ViewModel { get; set; }
    }
}