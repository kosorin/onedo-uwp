using OneDo.ViewModel.Core;

namespace OneDo.View.Core
{
    public interface IView
    {
        ExtendedViewModel ViewModel { get; set; }
    }

    public interface IView<TViewModel> : IView
    {
        TViewModel VM { get; }
    }
}