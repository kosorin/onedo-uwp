using System;
using System.Threading.Tasks;

namespace OneDo.Services.ProgressService
{
    public interface IProgressService
    {
        bool IsBusy { get; }

        void Run(Action action);

        Task RunAsync(Func<Task> action);
    }
}