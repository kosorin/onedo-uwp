using OneDo.Common.Logging;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace OneDo.Core.BackgroundTasks
{
    public class InProcessTestBackgroundTask : IBackgroundTask
    {
        public const string Name = nameof(InProcessTestBackgroundTask);

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            var details = taskInstance.TriggerDetails as ToastNotificationHistoryChangedTriggerDetail;
            if (details != null)
            {
                Logger.Current.Info($"Hello from in-process background task: {details.ChangeType}");
            }
            deferral.Complete();
        }
    }
}
