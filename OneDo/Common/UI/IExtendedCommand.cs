using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OneDo.Common.UI
{
    public interface IExtendedCommand : ICommand
    {
        void RaiseCanExecuteChanged();
    }
}
