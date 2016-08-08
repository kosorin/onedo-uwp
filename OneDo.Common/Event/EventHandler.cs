using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Common.Event
{
    public delegate void EventHandler<TSender, TEventArgs>(TSender sender, TEventArgs args);
}
