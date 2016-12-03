using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace OneDo.Services.BackgroundTaskService
{
    public interface IBackgroundTaskService
    {
        bool IsInitialized { get; }

        Task InitializeAsync();

        bool TryRegister<TBackgroundTask>(IBackgroundTrigger trigger) where TBackgroundTask : class, IBackgroundTask;

        bool TryRegister<TBackgroundTask>(IBackgroundTrigger trigger, BackgroundTaskParameters parameters) where TBackgroundTask : class, IBackgroundTask;

        bool TryRegister<TBackgroundTask>(IBackgroundTrigger trigger, IEnumerable<IBackgroundCondition> conditions) where TBackgroundTask : class, IBackgroundTask;

        bool TryRegister<TBackgroundTask>(IBackgroundTrigger trigger, IEnumerable<IBackgroundCondition> conditions, BackgroundTaskParameters parameters) where TBackgroundTask : class, IBackgroundTask;
    }
}