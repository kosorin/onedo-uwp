using GalaSoft.MvvmLight;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Services.ProgressService
{
    public class ProgressService : ViewModelBase, IProgressService
    {
        private readonly ConcurrentStack<int> stack = new ConcurrentStack<int>();

        public bool IsBusy => stack.Count > 0;

        public void Push()
        {
            stack.Push(default(int));
            RaisePropertyChanged(nameof(IsBusy));
        }

        public void Pop()
        {
            int i;
            stack.TryPop(out i);
            RaisePropertyChanged(nameof(IsBusy));
        }

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
    }
}
