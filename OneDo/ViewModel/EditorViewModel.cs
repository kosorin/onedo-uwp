using GalaSoft.MvvmLight;
using OneDo.Model.Data;
using OneDo.Services.ModalService;
using OneDo.Services.ProgressService;
using OneDo.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;

namespace OneDo.ViewModel
{
    public abstract class EditorViewModel<TModel> : ModalViewModel
    {
        private bool isNew;
        public bool IsNew
        {
            get { return isNew; }
            set
            {
                if (Set(ref isNew, value))
                {
                    UpdateDirtyProperty(() => value);
                }
            }
        }


        public bool CanSave => dirtyProperties.Any(x => x.Value) && validProperties.All(x => x.Value);


        public IProgressService ProgressService { get; }

        public AsyncRelayCommand SaveCommand { get; }

        public ICommand DeleteCommand { get; }

        private Dictionary<string, bool> dirtyProperties = new Dictionary<string, bool>();

        private Dictionary<string, bool> validProperties = new Dictionary<string, bool>();

        protected EditorViewModel(IModalService modalService, ISettingsProvider settingsProvider, IProgressService progressService)
            : base(modalService, settingsProvider)
        {
            ProgressService = progressService;

            SaveCommand = new AsyncRelayCommand(Save, () => CanSave);
            DeleteCommand = new AsyncRelayCommand(Delete, () => !IsNew);
        }


        protected abstract Task Save();

        protected abstract Task Delete();

        protected abstract void OnSaved();

        protected abstract void OnDeleted();


        protected void UpdateDirtyProperty(Func<bool> isDirtyTest, [CallerMemberName] string propertyName = null)
        {
            dirtyProperties[propertyName] = isDirtyTest();
            OnCanSave();
        }

        protected void ValidateProperty(Func<bool> isValidTest, [CallerMemberName] string propertyName = null)
        {
            validProperties[propertyName] = isValidTest();
            OnCanSave();
        }

        private void OnCanSave()
        {
            RaisePropertyChanged(nameof(CanSave));
            SaveCommand.RaiseCanExecuteChanged();
        }
    }
}
