using OneDo.ViewModel;

namespace OneDo.View
{
    public interface IXBind<TViewModel>
    {
        TViewModel VM { get; }
    }
}
