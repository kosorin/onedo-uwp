using OneDo.Common.UI;
using OneDo.Model.Args;
using OneDo.Model.Business;
using OneDo.Model.Data;
using OneDo.Services.ProgressService;
using OneDo.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace OneDo.ViewModel
{
    public abstract class ListViewModel<TEntity, TItem> : ModalViewModel
        where TEntity : class, IEntity
        where TItem : ItemObject<TEntity>
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

        public event TypedEventHandler<ListViewModel<TEntity, TItem>, EntityEventArgs<TEntity>> SelectionChanged;


        public IExtendedCommand ShowEditorCommand { get; }

        public IExtendedCommand DeleteCommand { get; }


        public DataService DataService { get; }

        public UIHost UIHost { get; }

        public IRepository<TEntity> Repository { get; }

        protected ListViewModel(DataService dataService, UIHost uiHost)
        {
            DataService = dataService;
            UIHost = uiHost;

            ShowEditorCommand = new RelayCommand<TItem>(ShowEditor);
            DeleteCommand = new AsyncRelayCommand<TItem>(Delete, CanDelete);

            Repository = DataService.GetRepository<TEntity>();
            Repository.Added += (_, args) => OnEntityAdded(args.Entity);
            Repository.Updated += (_, args) => OnEntityUpdated(args.Entity);
            Repository.Deleted += (_, args) => OnEntityDeleted(args.Entity);
            Repository.BulkDeleted += (_, args) => OnEntityBulkDeleted(args.Entities);
        }


        public void Clear()
        {
            Items.Clear();
        }

        public void Add(TEntity entity)
        {
            if (CanContain(entity))
            {
                var item = CreateItem(entity);
                Items.Add(item);
            }
        }

        public void Refresh(TEntity entity)
        {
            var item = GetItem(entity);
            if (item != null)
            {
                item.Refresh();
            }
        }

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

        protected virtual bool CanContain(TEntity entity)
        {
            return true;
        }


        protected void ShowEditor(TItem item)
        {
            var editor = CreateEditor(item);
            editor.Saved += (s, e) => UIHost.ModalService.Close();
            editor.Deleted += (s, e) => UIHost.ModalService.Close();
            UIHost.ModalService.Show(editor);
        }

        protected abstract EditorViewModel<TEntity> CreateEditor(TItem item);

        protected virtual async Task Delete(TItem item)
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                await Repository.Delete(item.Entity);
            });
        }

        protected virtual bool CanDelete(TItem item)
        {
            return true;
        }


        protected virtual void OnEntityAdded(TEntity entity)
        {
            Add(entity);
        }

        protected virtual void OnEntityUpdated(TEntity entity)
        {
            Refresh(entity);
        }

        protected virtual void OnEntityDeleted(TEntity entity)
        {
            Remove(entity);
        }

        protected virtual void OnEntityBulkDeleted(List<TEntity> entities)
        {
            var items = Items.Where(x => entities.Contains(x.Entity)).ToList();
            if (items.Count == Items.Count)
            {
                Clear();
            }
            else
            {
                foreach (var item in items)
                {
                    Items.Remove(item);
                }
            }
        }
    }
}
