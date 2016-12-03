using OneDo.Mvvm;
using System.ComponentModel;

namespace OneDo.ViewModel
{
    public interface INoteCommands : INotifyPropertyChanged
    {
        IExtendedCommand ShowEditorCommand { get; }

        IExtendedCommand DeleteCommand { get; }

        IExtendedCommand ToggleFlagCommand { get; }
    }
}