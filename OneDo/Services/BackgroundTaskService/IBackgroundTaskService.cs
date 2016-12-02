using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace OneDo.Services.BackgroundTaskService
{
    public interface IBackgroundTaskService
    {
        bool IsInitialized { get; }

        Task InitializeAsync();

        bool Register(string taskName, IBackgroundTrigger trigger);

        bool Register(string taskName, IBackgroundTrigger trigger, BackgroundTaskParameters parameters);

        bool Register(string taskName, IBackgroundTrigger trigger, IEnumerable<IBackgroundCondition> conditions);

        bool Register(string taskName, IBackgroundTrigger trigger, IEnumerable<IBackgroundCondition> conditions, BackgroundTaskParameters parameters);
    }
}