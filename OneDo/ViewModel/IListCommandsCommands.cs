using OneDo.Common.Mvvm;
using System.ComponentModel;

namespace OneDo.ViewModel
{
    public interface IListCommandsCommands : INotifyPropertyChanged
    {
        IExtendedCommand ShowEditorCommand { get; }

        IExtendedCommand DeleteCommand { get; }
    }
}