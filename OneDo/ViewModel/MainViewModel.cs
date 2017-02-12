using System.Windows.Input;
using System.Linq;
using System.Threading.Tasks;
using System;
using OneDo.Services.InfoService;
using OneDo.Common.Mvvm;
using OneDo.Application;
using OneDo.Application.Commands.Folders;
using OneDo.Application.Commands.Notes;
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


        public IApi Api { get; }

        public UIHost UIHost { get; }

        public MainViewModel(IApi api, UIHost uiHost, FolderListViewModel folderList, NoteListViewModel noteList)
        {
            Api = api;
            UIHost = uiHost;
            FolderList = folderList;
            NoteList = noteList;

            ShowSettingsCommand = new RelayCommand(ShowSettings);
        }

        public async Task Load()
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                await FolderList.Load();
            });
        }

#if DEBUG
        public async Task ResetData()
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                await Api.CommandBus.Execute(new DeleteAllFoldersCommand());
                await Api.SavePreviewData();

                await Load();
            });
        }
#endif

        private void ShowSettings()
        {
            Messenger.Default.Send(new ShowSettingsMessage());
        }
    }
}
