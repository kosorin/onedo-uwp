using GalaSoft.MvvmLight.Helpers;
using System;

namespace OneDo.ViewModel.Core.Command
{
    public class RelayCommand : IExtendedCommand
    {
        private readonly WeakAction execute;

        private readonly WeakFunc<bool> canExecute;

        public RelayCommand(Action execute) : this(execute, null)
        {

        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            this.execute = new WeakAction(execute);
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

        public void Execute(object parameter)
        {
            if (CanExecute(parameter) && execute != null && (execute.IsStatic || execute.IsAlive))
            {
                execute.Execute();
            }
        }
    }

    public class RelayCommand<T> : IExtendedCommand
    {
        private readonly WeakAction<T> execute;

        private readonly WeakFunc<T, bool> canExecute;

        public RelayCommand(Action<T> execute) : this(execute, null)
        {

        }

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            this.execute = new WeakAction<T>(execute);
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

        public void Execute(object parameter)
        {
            if (CanExecute(parameter) && execute != null && (execute.IsStatic || execute.IsAlive))
            {
                execute.Execute((T)parameter);
            }
        }
    }
}
