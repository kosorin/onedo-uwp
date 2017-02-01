using System.Windows.Input;
using System.Linq;
using System.Threading.Tasks;
using System;
using OneDo.Services.InfoService;
using OneDo.Services.ToastService;
using OneDo.Common.Mvvm;
using OneDo.Application;
using OneDo.Application.Commands.Folders;
using OneDo.Application.Commands.Notes;
using OneDo.ViewModel.Args;
using OneDo.Application.Queries.Notes;
using GalaSoft.MvvmLight.Messaging;
using OneDo.Core;
using OneDo.Core.Messages;

namespace OneDo.ViewModel
{
    public class MainViewModel : ExtendedViewModel
    {
        private FolderListViewModel folderList;
        public FolderListViewModel FolderList
        {
            get { return folderList; }
            set { Set(ref folderList, value); }
        }

        private NoteListViewModel noteList;
        public NoteListViewModel NoteList
        {
            get { return noteList; }
            set { Set(ref noteList, value); }
        }


        public ICommand ShowSettingsCommand { get; }


        public Api Api { get; }

        public UIHost UIHost { get; }

        public MainViewModel(Api api, UIHost uiHost, FolderListViewModel folderList, NoteListViewModel noteList)
        {
            Api = api;
            UIHost = uiHost;
            FolderList = folderList;
            NoteList = noteList;

            ShowSettingsCommand = new RelayCommand(ShowSettings);
        }

#if DEBUG
        private async Task Clear()
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                await Api.CommandBus.Execute(new DeleteAllFoldersCommand());
            });
        }

        public async Task ResetData()
        {
            await Clear();
            await FolderList.Load();
        }
#endif

        private void ShowSettings()
        {
            Messenger.Default.Send(new ShowSettingsMessage());
        }
    }
}
