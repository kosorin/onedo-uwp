using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Services.ProgressService
{
    public class ProgressService : ViewModelBase, IProgressService
    {
        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { Set(ref isBusy, value); }
        }
    }
}
