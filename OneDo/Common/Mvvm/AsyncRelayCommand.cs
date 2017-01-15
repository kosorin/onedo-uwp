using GalaSoft.MvvmLight.Helpers;
using System;
using System.Threading.Tasks;

namespace OneDo.Common.Mvvm
{
    public class AsyncRelayCommand : IExtendedCommand
    {
        private readonly WeakFunc<Task> execute;

        private readonly WeakFunc<bool> canExecute;

        public AsyncRelayCommand(Func<Task> execute) : this(execute, null)
        {

        }

        public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            this.execute = new WeakFunc<Task>(execute);
            if (canExecute != null)
            {
                this.canExecute = new WeakFunc<bool>(canExecute);
            }
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || (canExecute.IsStatic || canExecute.IsAlive) && canExecute.Execute();
        }

        public async void Execute(object parameter)
        {
            if (CanExecute(parameter) && execute != null && (execute.IsStatic || execute.IsAlive))
            {
                await execute.Execute();
            }
        }
    }

    public class AsyncRelayCommand<T> : IExtendedCommand
    {
        private readonly WeakFunc<T, Task> execute;

        private readonly WeakFunc<T, bool> canExecute;

        public AsyncRelayCommand(Func<T, Task> execute) : this(execute, null)
        {

        }

        public AsyncRelayCommand(Func<T, Task> execute, Func<T, bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            this.execute = new WeakFunc<T, Task>(execute);
            if (canExecute != null)
            {
                this.canExecute = new WeakFunc<T, bool>(canExecute);
            }
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || (canExecute.IsStatic || canExecute.IsAlive) && canExecute.Execute((T)parameter);
        }

        public async void Execute(object parameter)
        {
            if (CanExecute(parameter) && execute != null && (execute.IsStatic || execute.IsAlive))
            {
                await execute.Execute((T)parameter);
            }
        }
    }
}
