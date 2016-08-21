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
                    SetDirtyProperty(() => value);
                }
            }
        }

        private Dictionary<string, bool> dirtyProperties = new Dictionary<string, bool>();
        public bool IsDirty => dirtyProperties.Any(x => x.Value);


        public IProgressService ProgressService { get; }


        public ICommand SaveCommand { get; }

        public ICommand DeleteCommand { get; }


        protected EditorViewModel(IModalService modalService, ISettingsProvider settingsProvider, IProgressService progressService)
            : base(modalService, settingsProvider)
        {
            ProgressService = progressService;

            SaveCommand = new AsyncRelayCommand(Save);
            DeleteCommand = new AsyncRelayCommand(Delete);
        }


        protected abstract Task Save();

        protected abstract Task Delete();

        protected abstract void OnSaved();

        protected abstract void OnDeleted();


        protected void SetDirtyProperty(Func<bool> isDirtyTest, [CallerMemberName] string propertyName = null)
        {
            dirtyProperties[propertyName] = isDirtyTest();
            RaisePropertyChanged(nameof(IsDirty));
        }
    }
}
