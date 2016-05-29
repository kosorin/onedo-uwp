using GalaSoft.MvvmLight;

namespace OneDo.Views
{
    public interface IView
    {
        ViewModelBase ViewModel { get; set; }
    }
}