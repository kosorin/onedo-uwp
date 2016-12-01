using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace OneDo.Services.BackgroundTaskService
{
    public interface IBackgroundTaskService
    {
        bool IsInitialized { get; }

        Task InitializeAsync();

        bool RegisterTask(string taskName, IBackgroundTrigger trigger);
    }
}