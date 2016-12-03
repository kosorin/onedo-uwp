using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Services.ModalService
{
    public interface IModal
    {
        IModalService SubModalService { get; }
    }
}
