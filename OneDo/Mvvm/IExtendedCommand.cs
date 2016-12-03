using System.Windows.Input;

namespace OneDo.Mvvm
{
    public interface IExtendedCommand : ICommand
    {
        void RaiseCanExecuteChanged();
    }
}
