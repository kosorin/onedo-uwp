using OneDo.Application;
using OneDo.Application.Common;
using OneDo.Common.Mvvm;
using OneDo.ViewModel.Args;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;

namespace OneDo.ViewModel
{
    public abstract class ListViewModel<TEntityModel, TItem> : ModalViewModel
        where TEntityModel : class, IEntityModel
        where TItem : ItemObject<TEntityModel>
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
                    SelectionChanged?.Invoke(this, new EntityModelEventArgs<TEntityModel>(SelectedItem?.EntityModel));
                }
            }
        }

        public event TypedEventHandler<ListViewModel<TEntityModel, TItem>, EntityEventArgs<TEntityModel>> SelectionChanged;


        public IExtendedCommand ShowEditorCommand { get; }

        public IExtendedCommand DeleteCommand { get; }


        public Api Api { get; }

        public UIHost UIHost { get; }

        public IRepository<TEntity> Repository { get; }

        protected ListViewModel(DataService dataService, UIHost uiHost)
        {
            Api = dataService;
            UIHost = uiHost;

            ShowEditorCommand = new RelayCommand<TItem>(ShowEditor);
            DeleteCommand = new AsyncRelayCommand<TItem>(Delete, CanDelete);

            Repository = Api.GetRepository<TEntity>();
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
            return Items.FirstOrDefault(x => x.EntityModel.Id == entity.Id);
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
                await Repository.Delete(item.EntityModel);
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
            var items = Items.Where(x => entities.Contains(x.EntityModel)).ToList();
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
