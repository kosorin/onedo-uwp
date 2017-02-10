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
using OneDo.Core.CommandMessages;
using OneDo.Common.Extensions;
using System.Collections.Specialized;

namespace OneDo.ViewModel
{
    public class FolderListViewModel : ListViewModel<FolderItemViewModel, FolderModel>
    {
        public FolderListViewModel(IApi api, UIHost uiHost) : base(api, uiHost)
        {
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

        private FolderItemViewModel CreateItem(FolderModel entity)
        {
            return new FolderItemViewModel(entity, Api, UIHost);
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
    }
}
