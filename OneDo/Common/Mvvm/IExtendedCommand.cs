using System.Windows.Input;

namespace OneDo.Common.Mvvm
{
    public interface IExtendedCommand : ICommand
    {
        void RaiseCanExecuteChanged();
    }
}
