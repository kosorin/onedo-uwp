namespace OneDo.Views
{
    public interface IXBind<TViewModel>
    {
        TViewModel VM { get; }
    }
}
