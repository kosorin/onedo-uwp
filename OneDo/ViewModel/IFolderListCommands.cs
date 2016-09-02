using GalaSoft.MvvmLight.Command;
using OneDo.ViewModel.Commands;
using OneDo.ViewModel.Items;
using System.ComponentModel;

namespace OneDo.ViewModel
{
    public interface IFolderListCommands : INotifyPropertyChanged
    {
        RelayCommand AddItemCommand { get; }

        RelayCommand<FolderItemObject> EditItemCommand { get; }

        AsyncRelayCommand<FolderItemObject> DeleteItemCommand { get; }
    }
}