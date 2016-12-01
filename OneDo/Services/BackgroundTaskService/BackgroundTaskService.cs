using OneDo.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace OneDo.Services.BackgroundTaskService
{
    public class BackgroundTaskService : IBackgroundTaskService
    {
        public bool IsInitialized { get; private set; }

        public async Task InitializeAsync()
        {
            if (!IsInitialized)
            {
                IsInitialized = await RequestAccessAsync();
            }
        }

        private async Task<bool> RequestAccessAsync()
        {
            var status = await BackgroundExecutionManager.RequestAccessAsync();
            return status != BackgroundAccessStatus.DeniedBySystemPolicy
                && status != BackgroundAccessStatus.DeniedByUser;
        }

        public bool RegisterTask(string taskName, IBackgroundTrigger trigger)
        {
            if (IsInitialized)
            {
                try
                {
                    if (BackgroundTaskRegistration.AllTasks.All(t => t.Value.Name != taskName))
                    {
                        var builder = new BackgroundTaskBuilder
                        {
                            Name = taskName,
                        };
                        builder.SetTrigger(trigger);
                        builder.Register();
                        return true;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.Current.Error($"Couldn't register task '{taskName}'", e);
                }
            }
            return false;
        }
    }
}
