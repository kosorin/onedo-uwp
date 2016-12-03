using GalaSoft.MvvmLight;
using OneDo.Model.Data;
using OneDo.Services.ModalService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
