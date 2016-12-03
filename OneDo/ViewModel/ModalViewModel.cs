using OneDo.Mvvm;
using OneDo.Services.ModalService;

namespace OneDo.ViewModel
{
    public abstract class ModalViewModel : ExtendedViewModel, IModal
    {
        public static ModalViewModel Null { get; } = new NullModalViewModel();

        public SubModalService SubModalService { get; } = new SubModalService();

        IModalService IModal.SubModalService => SubModalService;

        private class NullModalViewModel : ModalViewModel
        {
            public NullModalViewModel()
            {

            }
        }
    }
}
