using OneDo.Services.ProgressService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using OneDo.Model;
using OneDo.Model.Entities.Args;
using OneDo.Common.Mvvm;

namespace OneDo.ViewModel
{
    public abstract class EditorViewModel<TEntity> : ModalViewModel where TEntity : IEntity
    {
        private bool isNew;
        public bool IsNew
        {
            get { return isNew; }
            set
            {
                if (Set(ref isNew, value))
                {
                    UpdateDirtyProperty(() => IsNew);
                    DeleteCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public bool CanSave => dirtyProperties.Any(x => x.Value) && validProperties.All(x => x.Value);


        public event EventHandler<EntityEventArgs<TEntity>> Saved;

        public event EventHandler<EntityEventArgs<TEntity>> Deleted;


        public AsyncRelayCommand SaveCommand { get; }

        public AsyncRelayCommand DeleteCommand { get; }


        public IProgressService ProgressService { get; }

        public TEntity Original { get; protected set; }

        private Dictionary<string, bool> dirtyProperties = new Dictionary<string, bool>();

        private Dictionary<string, bool> validProperties = new Dictionary<string, bool>();

        protected EditorViewModel(IProgressService progressService)
        {
            ProgressService = progressService;

            SaveCommand = new AsyncRelayCommand(Save, () => CanSave);
            DeleteCommand = new AsyncRelayCommand(Delete, () => !IsNew);
        }


        protected void UpdateDirtyProperty(Func<bool> isDirtyTest, [CallerMemberName] string propertyName = null)
        {
            dirtyProperties[propertyName] = isDirtyTest();
            RaiseCanSaveChanged();
        }

        protected void ValidateProperty(Func<bool> isValidTest, [CallerMemberName] string propertyName = null)
        {
            validProperties[propertyName] = isValidTest();
            RaiseCanSaveChanged();
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

        private void RaiseCanSaveChanged()
        {
            RaisePropertyChanged(nameof(CanSave));
            SaveCommand.RaiseCanExecuteChanged();
        }
    }
}
