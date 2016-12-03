using OneDo.Common.Logging;
using Windows.ApplicationModel.Background;

namespace OneDo.Core.BackgroundTasks
{
    public class InProcessTestBackgroundTask : IBackgroundTask
    {
        public const string Name = nameof(InProcessTestBackgroundTask);

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            Logger.Current.Info("Hello from in-process background task");
        }
    }
}
