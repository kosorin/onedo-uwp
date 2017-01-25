using OneDo.Services.ProgressService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using OneDo.Common.Mvvm;
using OneDo.Application.Common;
using OneDo.ViewModel.Args;
using OneDo.Application;

namespace OneDo.ViewModel
{
    public abstract class EditorViewModel<TEntity> : ModalViewModel
        where TEntity : class, IEntityModel, new()
    {
        private Guid? id;
        public Guid? Id
        {
            get { return id; }
            private set
            {
                if (Set(ref id, value))
                {
                    RaisePropertyChanged(nameof(IsNew));
                    MarkProperty(() => IsNew);
                    DeleteCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public bool IsNew => Id == null;

        public bool CanSave => dirtyProperties.Any(x => x.Value) && validProperties.All(x => x.Value);


        protected Dictionary<string, Func<bool>> Rules { get; set; } = new Dictionary<string, Func<bool>>();


        public event EventHandler<EntityEventArgs<TEntity>> Saved;

        public event EventHandler<EntityEventArgs<TEntity>> Deleted;


        public AsyncRelayCommand SaveCommand { get; }

        public AsyncRelayCommand DeleteCommand { get; }


        public Api Api { get; }

        public IProgressService ProgressService { get; }

        public TEntity Original { get; protected set; }

        private Dictionary<string, bool> dirtyProperties = new Dictionary<string, bool>();

        private Dictionary<string, bool> validProperties = new Dictionary<string, bool>();

        private bool isInitialized;

        protected EditorViewModel(Api api, IProgressService progressService)
        {
            Api = api;
            ProgressService = progressService;

            Original = CreateDefault();

            SaveCommand = new AsyncRelayCommand(Save, () => CanSave);
            DeleteCommand = new AsyncRelayCommand(Delete, () => Id != null);
        }

        public async Task Load(Guid? id)
        {
            Id = id;
            if (Id != null)
            {
                await InitializeData();
            }
            InitializeProperties();

            isInitialized = true;
            ValidateAll();
        }

        protected abstract Task InitializeData();

        protected abstract void InitializeProperties();

        protected virtual TEntity CreateDefault()
        {
            return new TEntity();
        }


        protected void MarkProperty(Func<bool> isDirtyTest, [CallerMemberName] string propertyName = null)
        {
            if (isInitialized)
            {
                dirtyProperties[propertyName] = isDirtyTest();
                RaiseCanSaveChanged();
            }
        }

        protected void ValidateProperty([CallerMemberName] string propertyName = null)
        {
            if (isInitialized)
            {
                Func<bool> rule;
                if (Rules.TryGetValue(propertyName, out rule))
                {
                    validProperties[propertyName] = rule();
                    RaiseCanSaveChanged();
                }
            }
        }

        protected void ValidateAll()
        {
            if (isInitialized)
            {
                foreach (var ruleItem in Rules)
                {
                    validProperties[ruleItem.Key] = ruleItem.Value();
                }
                RaiseCanSaveChanged();
            }
        }

        private void RaiseCanSaveChanged()
        {
            RaisePropertyChanged(nameof(CanSave));
            SaveCommand.RaiseCanExecuteChanged();
        }


        protected abstract Task Save();

        protected abstract Task Delete();

        protected void OnSaved()
        {
            Saved?.Invoke(this, new EntityEventArgs<TEntity>(Original));
        }

        protected void OnDeleted()
        {
            Deleted?.Invoke(this, new EntityEventArgs<TEntity>(Original));
        }
    }
}
