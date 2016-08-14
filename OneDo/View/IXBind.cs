using OneDo.ViewModel;

namespace OneDo.View
{
    public interface IXBind<TViewModel> where TViewModel : ExtendedViewModel
    {
        TViewModel VM { get; }
    }
}
