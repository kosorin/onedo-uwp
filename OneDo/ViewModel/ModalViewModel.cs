using OneDo.Common.Mvvm;
using OneDo.Services.ModalService;

namespace OneDo.ViewModel
{
    public abstract class ModalViewModel : ExtendedViewModel
    {
        public static ModalViewModel Null { get; } = new NullModalViewModel();

        public ModalViewModel Child { get; set; }

        private class NullModalViewModel : ModalViewModel
        {
        }
    }
}
