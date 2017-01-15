using OneDo.Common.Mvvm;
using System.ComponentModel;

namespace OneDo.ViewModel
{
    public interface IFolderCommands : INotifyPropertyChanged
    {
        IExtendedCommand ShowEditorCommand { get; }

        IExtendedCommand DeleteCommand { get; }
    }
}