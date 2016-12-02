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

        public static void Run<TBackgroundTask>(IBackgroundTaskInstance taskInstance) where TBackgroundTask : IBackgroundTask
        {
            Activator.CreateInstance<TBackgroundTask>().Run(taskInstance);
        }

        public bool Register(string taskName, IBackgroundTrigger trigger)
        {
            return Register(taskName, trigger, Enumerable.Empty<IBackgroundCondition>(), BackgroundTaskParameters.None);
        }

        public bool Register(string taskName, IBackgroundTrigger trigger, BackgroundTaskParameters parameters)
        {
            return Register(taskName, trigger, Enumerable.Empty<IBackgroundCondition>(), parameters);
        }

        public bool Register(string taskName, IBackgroundTrigger trigger, IEnumerable<IBackgroundCondition> conditions)
        {
            return Register(taskName, trigger, conditions, BackgroundTaskParameters.None);
        }

        public bool Register(string taskName, IBackgroundTrigger trigger, IEnumerable<IBackgroundCondition> conditions, BackgroundTaskParameters parameters)
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
                            CancelOnConditionLoss = parameters.HasFlag(BackgroundTaskParameters.CancelOnConditionLoss),
                            IsNetworkRequested = parameters.HasFlag(BackgroundTaskParameters.IsNetworkRequested),
                        };
                        builder.SetTrigger(trigger);
                        foreach (var condition in conditions)
                        {
                            builder.AddCondition(condition);
                        }
                        builder.Register();
                        Logger.Current.Info($"Background task '{taskName}' was registered");
                        return true;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.Current.Error($"Couldn't register background task '{taskName}'", e);
                }
            }
            return false;
        }
    }
}
