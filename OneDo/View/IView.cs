using OneDo.Mvvm;

namespace OneDo.View
{
    public interface IView
    {
        ExtendedViewModel ViewModel { get; set; }
    }
}