using OneDo.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Core.Args
{
    public class SelectionChangedEventArgs<TItem> : EventArgs
    {
        public TItem Item { get; }

        public SelectionChangedEventArgs(TItem item)
        {
            Item = item;
        }
    }
}