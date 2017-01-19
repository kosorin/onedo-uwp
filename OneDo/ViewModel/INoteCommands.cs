using OneDo.Common.Mvvm;
using System.ComponentModel;

namespace OneDo.ViewModel
{
    public interface INoteCommands : IListCommandsCommands
    {
        IExtendedCommand ToggleFlagCommand { get; }
    }
}