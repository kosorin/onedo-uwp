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

        public IToastService ToastService { get; }

        public MainViewModel(Api api, UIHost uiHost, IToastService toastService)
        {
            Api = api;
            UIHost = uiHost;
            ToastService = toastService;

            FolderList = new FolderListViewModel(Api, UIHost);
            NoteList = new NoteListViewModel(Api, UIHost, FolderList);

            ShowSettingsCommand = new RelayCommand(ShowSettings);
        }

        public async Task Load()
        {
#if DEBUG
            var folders = await Api.FolderQuery.GetAll();
            if (!folders.Any())
            {
                await Api.CommandBus.Execute(new SaveFolderCommand(Guid.Empty, "Inbox", "#0063AF"));
                await Api.CommandBus.Execute(new SaveFolderCommand(Guid.Empty, "Work", "#0F893E"));
                await Api.CommandBus.Execute(new SaveFolderCommand(Guid.Empty, "Shopping list", "#AC008C"));
                await Api.CommandBus.Execute(new SaveFolderCommand(Guid.Empty, "Vacation", "#F7630D"));
                folders = await Api.FolderQuery.GetAll();

                var folder = folders.FirstOrDefault();
                var folder2 = folders.Skip(1).FirstOrDefault();
                await Api.CommandBus.Execute(new SaveNoteCommand(Guid.Empty, folder.Id, "Buy milk", "", null, null, false));
                await Api.CommandBus.Execute(new SaveNoteCommand(Guid.Empty, folder.Id, "Walk Max with bike", "", DateTime.Today, TimeSpan.FromHours(7.25), false));
                await Api.CommandBus.Execute(new SaveNoteCommand(Guid.Empty, folder.Id, "Call mom", "", DateTime.Today.AddDays(5), null, true));
                await Api.CommandBus.Execute(new SaveNoteCommand(Guid.Empty, folder.Id,
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit",
                    "Proin et diam at lorem egestas ullamcorper. Curabitur non eleifend mi. Praesent eu sem elementum, rutrum neque id, sollicitudin dolor. Proin molestie ullamcorper sem a hendrerit. Integer ac sapien erat. Morbi vehicula venenatis dolor, non aliquet nibh mattis sed.",
                    null, null, false));
                await Api.CommandBus.Execute(new SaveNoteCommand(Guid.Empty, folder2.Id, "Test note", "", null, null, true));
            }

            FolderList.SelectedItem = FolderList.Items.FirstOrDefault();
#endif
            await FolderList.Load();
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
            await Load();
        }
#endif

        private void ShowSettings()
        {
            UIHost.ModalService.Show(new SettingsViewModel());
        }
    }
}
