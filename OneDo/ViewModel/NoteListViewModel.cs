using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using OneDo.Common.Mvvm;
using OneDo.Application.Queries.Notes;
using OneDo.Application;
using System;
using OneDo.Application.Commands.Notes;
using OneDo.Application.Queries.Folders;
using OneDo.Core;
using GalaSoft.MvvmLight.Messaging;
using OneDo.Core.Messages;
using OneDo.Core.Args;
using OneDo.Common.Extensions;

namespace OneDo.ViewModel
{
    public class NoteListViewModel : ListViewModel<NoteItemViewModel, NoteModel>
    {
        public FolderListViewModel FolderList { get; set; }

        public NoteListViewModel(IApi api, UIHost uiHost, FolderListViewModel folderList) : base(api, uiHost)
        {
            FolderList = folderList;
            FolderList.SelectionChanged += FolderList_SelectionChanged;
        }

        private async void FolderList_SelectionChanged(object sender, SelectionChangedEventArgs<FolderItemViewModel> args)
        {
            if (args.Item != null)
            {
                await Load(args.Item.Id);
            }
            else
            {
                Items.Clear();
            }
        }

        private async Task Load(Guid folderId)
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                var notes = await Api.NoteQuery.GetAll(folderId);
                Items = notes.Select(CreateItem).ToObservableCollection();
            });
        }

        private NoteItemViewModel CreateItem(NoteModel entity)
        {
            return new NoteItemViewModel(entity, Api, UIHost);
        }


        protected override void ShowEditor()
        {
            Messenger.Default.Send(new ShowNoteEditorMessage(null));
        }
    }
}
