using OneDo.Application;
using OneDo.Application.Queries.Folders;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System;
using OneDo.Application.Commands.Folders;
using OneDo.Core;
using GalaSoft.MvvmLight.Messaging;
using OneDo.Core.Messages;
using OneDo.Common.Extensions;
using System.Collections.Specialized;
using OneDo.Application.Events;
using OneDo.Application.Events.Folders;
using OneDo.Application.Models;
using OneDo.ViewModel.Core;

namespace OneDo.ViewModel.Folder
{
    public class FolderListViewModel : ListViewModel<FolderItemViewModel, FolderModel>
    {
        public FolderListViewModel(IApi api, UIHost uiHost) : base(api, uiHost)
        {
            Api.EventBus.Subscribe<FolderAddedEvent>(Folder_Added);
            Api.EventBus.Subscribe<FolderDeletedEvent>(Folder_Deleted);
        }

        public async Task Load()
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                var folders = await Api.FolderQuery.GetAll();
                Items = folders.Select(CreateItem).ToObservableCollection();
                Items.CollectionChanged += Items_CollectionChanged;
                UpdateDeletePermissions();

                SelectedItem = Items.FirstOrDefault();
            });
        }

        private FolderItemViewModel CreateItem(FolderModel model)
        {
            return new FolderItemViewModel(model, Api, UIHost);
        }

        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateDeletePermissions();
        }

        private void UpdateDeletePermissions()
        {
            foreach (var item in Items)
            {
                item.AllowDelete = Items.Count > 1;
            }
        }

        protected override void ShowEditor()
        {
            Messenger.Default.Send(new ShowFolderEditorMessage(null));
        }


        private void Folder_Added(FolderAddedEvent @event)
        {
            Items.Add(CreateItem(@event.Model));
        }

        private void Folder_Deleted(FolderDeletedEvent @event)
        {
            var item = Items.FirstOrDefault(x => x.Id == @event.Id);
            var selectNew = item == SelectedItem;
            Items.Remove(item);
            if (selectNew)
            {
                SelectedItem = Items.FirstOrDefault();
            }
        }
    }
}
