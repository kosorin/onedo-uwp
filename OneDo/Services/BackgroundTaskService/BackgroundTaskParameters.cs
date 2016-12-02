using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Services.BackgroundTaskService
{
    [Flags]
    public enum BackgroundTaskParameters
    {
        None = 0,
        CancelOnConditionLoss = 1,
        IsNetworkRequested = 2,
    }
}
