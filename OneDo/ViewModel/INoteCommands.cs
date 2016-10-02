using GalaSoft.MvvmLight.Command;
using OneDo.ViewModel.Commands;
using OneDo.ViewModel.Items;
using System.ComponentModel;

namespace OneDo.ViewModel
{
    public interface INoteCommands : INotifyPropertyChanged
    {
        RelayCommand AddCommand { get; }

        RelayCommand<NoteItemObject> EditCommand { get; }

        AsyncRelayCommand<NoteItemObject> DeleteCommand { get; }
    }
}