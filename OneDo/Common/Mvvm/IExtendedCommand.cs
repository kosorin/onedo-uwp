using System.Windows.Input;

namespace OneDo.Common.Mvvm
{
    // TODO: IExtendedCommand přejmenovat pouze na ICommand a všechny původní ICommandy nahradit novým
    public interface IExtendedCommand : ICommand
    {
        void RaiseCanExecuteChanged();
    }
}
