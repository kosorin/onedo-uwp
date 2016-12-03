using System;

namespace OneDo.Services.BackgroundTaskService
{
    [Flags]
    public enum BackgroundTaskParameters
    {
        None = 0,
        CancelOnConditionLoss = 1,
        IsNetworkRequested = 2,
        IsOutProcess = 4,
    }
}
