using OneDo.Common.Mvvm;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace OneDo.Services.ProgressService
{
    public class ProgressService : ExtendedViewModel, IProgressService
    {
        private readonly ConcurrentStack<int> stack = new ConcurrentStack<int>();

        public bool IsBusy => stack.Count > 0;

        public void Run(Action action)
        {
            try
            {
                Push();
                action();
            }
            finally
            {
                Pop();
            }
        }

        public async Task RunAsync(Func<Task> action)
        {
            try
            {
                Push();
                await action();
            }
            finally
            {
                Pop();
            }
        }

        private void Push()
        {
            stack.Push(default(int));
            RaisePropertyChanged(nameof(IsBusy));
        }

        private void Pop()
        {
            int i;
            stack.TryPop(out i);
            RaisePropertyChanged(nameof(IsBusy));
        }
    }
}
