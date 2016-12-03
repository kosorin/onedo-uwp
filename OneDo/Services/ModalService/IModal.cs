namespace OneDo.Services.ModalService
{
    public interface IModal
    {
        IModalService SubModalService { get; }
    }
}
