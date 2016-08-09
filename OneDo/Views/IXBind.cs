using OneDo.ViewModels;

namespace OneDo.Views
{
    public interface IXBind<TViewModel> where TViewModel : ExtendedViewModel
    {
        TViewModel VM { get; }
    }
}
