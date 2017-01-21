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
                    SelectionChanged?.Invoke(this, new EntityEventArgs<TEntity>(SelectedItem?.Entity));
                }
            }
        }


        public event EventHandler<EntityEventArgs<TEntity>> SelectionChanged;


        public IExtendedCommand ShowEditorCommand { get; }

        public IExtendedCommand DeleteCommand { get; }


        public Api Api { get; }

        public UIHost UIHost { get; }

        protected ListViewModel(Api api, UIHost uiHost)
        {
            Api = api;
            UIHost = uiHost;

            ShowEditorCommand = new RelayCommand<TItem>(ShowEditor);
            DeleteCommand = new AsyncRelayCommand<TItem>(Delete, CanDelete);
        }

        [Obsolete]
        public void Clear()
        {
            Items.Clear();
        }

        [Obsolete]
        public void Add(TEntity entity)
        {
            var item = CreateItem(entity);
            Items.Add(item);
        }

        [Obsolete]
        public void Refresh(TEntity entity)
        {
            var item = GetItem(entity);
            if (item != null)
            {
                item.Refresh();
            }
        }

        [Obsolete]
        public void Remove(TEntity entity)
        {
            var item = GetItem(entity);
            if (item != null)
            {
                Items.Remove(item);
            }
        }


        protected TItem GetItem(TEntity entity)
        {
            return Items.FirstOrDefault(x => x.Entity.Id == entity.Id);
        }

        protected abstract TItem CreateItem(TEntity entity);


        protected void ShowEditor(TItem item)
        {
            Messenger.Default.Send(CreateShowEditorMessage(item?.Entity));
        }

        protected abstract ShowEntityEditorMessage<TEntity> CreateShowEditorMessage(TEntity entity);


        protected abstract Task Delete(TItem item);

        protected abstract bool CanDelete(TItem item);
    }
}
