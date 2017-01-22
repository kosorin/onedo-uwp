using GalaSoft.MvvmLight.Messaging;
using OneDo.Application;
using OneDo.Application.Common;
using OneDo.Common.Mvvm;
using OneDo.ViewModel.Args;
using OneDo.ViewModel.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;

namespace OneDo.ViewModel
{
    public abstract class ListViewModel<TItem, TEntity> : ModalViewModel, IListCommandsCommands
        where TItem : ItemViewModel<TEntity>
        where TEntity : class, IEntityModel, new()
    {
        private ObservableCollection<TItem> items;
        public ObservableCollection<TItem> Items
        {
            get
            {
                if (items == null)
                {
                    items = new ObservableCollection<TItem>();
                }
                return items;
            }
            set { Set(ref items, value); }
        }

        private TItem selectedItem;
        public TItem SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (Set(ref selectedItem, value))
                {
                    SelectionChanged?.Invoke(this, new SelectionChangedEventArgs<TItem>(SelectedItem));
                }
            }
        }


        public event EventHandler<SelectionChangedEventArgs<TItem>> SelectionChanged;


        public IExtendedCommand ShowEditorCommand { get; }

        public IExtendedCommand DeleteCommand { get; }


        public Api Api { get; }

        public UIHost UIHost { get; }

        protected ListViewModel(Api api, UIHost uiHost)
        {
            Api = api;
            UIHost = uiHost;

            ShowEditorCommand = new AsyncRelayCommand<TItem>(ShowEditor);
            DeleteCommand = new AsyncRelayCommand<TItem>(Delete, CanDelete);
        }


        protected abstract TItem CreateItem(TEntity entity);

        protected abstract Task<TEntity> GetEntity(TItem item);


        protected async Task ShowEditor(TItem item)
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                var entity = await GetEntity(item);
                if (entity != null)
                {
                    var message = CreateShowEditorMessage(entity);
                    Messenger.Default.Send(message);
                }
            });
        }

        protected abstract ShowEntityEditorMessage<TEntity> CreateShowEditorMessage(TEntity entity);


        protected abstract Task Delete(TItem item);

        protected abstract bool CanDelete(TItem item);
    }
}
