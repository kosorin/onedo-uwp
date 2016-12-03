using OneDo.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public static void Run<TBackgroundTask>(IBackgroundTaskInstance taskInstance) where TBackgroundTask : class, IBackgroundTask
        {
            Activator.CreateInstance<TBackgroundTask>().Run(taskInstance);
        }

        public bool TryRegister<TBackgroundTask>(IBackgroundTrigger trigger) where TBackgroundTask : class, IBackgroundTask
        {
            return TryRegister<TBackgroundTask>(trigger, Enumerable.Empty<IBackgroundCondition>(), BackgroundTaskParameters.None);
        }

        public bool TryRegister<TBackgroundTask>(IBackgroundTrigger trigger, BackgroundTaskParameters parameters) where TBackgroundTask : class, IBackgroundTask
        {
            return TryRegister<TBackgroundTask>(trigger, Enumerable.Empty<IBackgroundCondition>(), parameters);
        }

        public bool TryRegister<TBackgroundTask>(IBackgroundTrigger trigger, IEnumerable<IBackgroundCondition> conditions) where TBackgroundTask : class, IBackgroundTask
        {
            return TryRegister<TBackgroundTask>(trigger, conditions, BackgroundTaskParameters.None);
        }

        public bool TryRegister<TBackgroundTask>(IBackgroundTrigger trigger, IEnumerable<IBackgroundCondition> conditions, BackgroundTaskParameters parameters) where TBackgroundTask : class, IBackgroundTask
        {
            if (!IsInitialized)
            {
                return false;
            }

            var name = GetTaskName<TBackgroundTask>();
            try
            {
                if (!IsTaskRegistered(name))
                {
                    var entryPoint = GetTaskEntryPoint<TBackgroundTask>();
                    var builder = CreateTaskBuilder(name, entryPoint, trigger, conditions, parameters);
                    builder.Register();
                    Logger.Current.Info($"Register background task '{name}'");
                    return true;
                }
                else
                {
                    Logger.Current.Info($"Background task '{name}' already registered");
                    return true;
                }
            }
            catch (Exception e)
            {
                Logger.Current.Error($"Cannot register background task '{name}'", e);
                return false;
            }
        }

        private string GetTaskName<TBackgroundTask>() where TBackgroundTask : class, IBackgroundTask
        {
            return typeof(TBackgroundTask).Name;
        }

        private string GetTaskEntryPoint<TBackgroundTask>() where TBackgroundTask : class, IBackgroundTask
        {
            return $"{typeof(TBackgroundTask).GetTypeInfo().Assembly.GetName().Name}.{typeof(TBackgroundTask).Name}";
        }

        private bool IsTaskRegistered(string name)
        {
            return BackgroundTaskRegistration.AllTasks.Values.Any(t => t.Name == name);
        }

        private BackgroundTaskBuilder CreateTaskBuilder(string name, string entryPoint, IBackgroundTrigger trigger, IEnumerable<IBackgroundCondition> conditions, BackgroundTaskParameters parameters)
        {
            var builder = new BackgroundTaskBuilder
            {
                Name = name,
                CancelOnConditionLoss = parameters.HasFlag(BackgroundTaskParameters.CancelOnConditionLoss),
                IsNetworkRequested = parameters.HasFlag(BackgroundTaskParameters.IsNetworkRequested),
            };

            if (parameters.HasFlag(BackgroundTaskParameters.IsOutProcess))
            {
                builder.TaskEntryPoint = entryPoint;
            }

            builder.SetTrigger(trigger);

            foreach (var condition in conditions)
            {
                builder.AddCondition(condition);
            }

            return builder;
        }
    }
}
