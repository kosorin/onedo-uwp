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
    public abstract class ModalViewModel : ExtendedViewModel
    {
        public static ModalViewModel Null { get; } = new NullModalViewModel();

        public IModalService ModalService { get; }

        public DataService DataService { get; }

        protected ModalViewModel(IModalService modalService, DataService dataService)
        {
            ModalService = modalService;
            DataService = dataService;
        }

        private class NullModalViewModel : ModalViewModel
        {
            public NullModalViewModel() : base(null, null)
            {

            }
        }
    }
}
