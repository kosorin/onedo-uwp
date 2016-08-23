using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModel
{
    public interface IItemObject<TEntity>
    {
        TEntity Entity { get; }

        void Refresh();
    }
}
