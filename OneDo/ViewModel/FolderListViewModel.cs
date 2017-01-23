﻿using OneDo.Application;
using OneDo.Application.Queries.Folders;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System;
using OneDo.Application.Commands.Folders;
using OneDo.ViewModel.Parameters;
using OneDo.ViewModel.Items;

namespace OneDo.ViewModel
{
    public class FolderListViewModel : ListViewModel<FolderItemViewModel, FolderModel>, IFolderCommands
    {
        public FolderListViewModel(Api api, UIHost uiHost) : base(api, uiHost)
        {
        }

        public async Task Load()
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                var folders = await Api.FolderQuery.GetAll();
                Items = new ObservableCollection<FolderItemViewModel>(folders.Select(CreateItem));
                Items.CollectionChanged += (s, e) =>
                {
                    DeleteCommand.RaiseCanExecuteChanged();
                };
                SelectedItem = Items.FirstOrDefault();
            });
        }


        protected override FolderItemViewModel CreateItem(FolderModel entity)
        {
            return new FolderItemViewModel(entity, this);
        }


        protected override IParameters GetEditorParameters(Guid? id)
        {
            return new FolderEditorParameters(id);
        }


        protected override async Task Delete(FolderItemViewModel item)
        {
            await Api.CommandBus.Execute(new DeleteFolderCommand(item.Id));
        }

        protected override bool CanDelete(FolderItemViewModel item)
        {
            return Items.Count > 1;
        }
    }
}
