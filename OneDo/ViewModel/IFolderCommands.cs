using GalaSoft.MvvmLight.Command;
using OneDo.ViewModel.Commands;
using OneDo.ViewModel.Items;
using System.ComponentModel;

namespace OneDo.ViewModel
{
    public interface IFolderCommands : INotifyPropertyChanged
    {
        RelayCommand AddCommand { get; }

        RelayCommand<FolderItemObject> EditCommand { get; }

        AsyncRelayCommand<FolderItemObject> DeleteCommand { get; }
    }
}