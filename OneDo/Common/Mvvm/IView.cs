
namespace OneDo.Common.Mvvm
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