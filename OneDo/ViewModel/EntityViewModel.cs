using GalaSoft.MvvmLight;
using OneDo.Application;
using OneDo.Application.Common;
using OneDo.Common.Mvvm;
using OneDo.Core;
using System;
using System.Threading.Tasks;

namespace OneDo.ViewModel
{
    public abstract class EntityViewModel<TEntity> : ObservableObject
        where TEntity : IEntityModel
    {
        public Guid Id { get; }


        public IExtendedCommand ShowEditorCommand { get; }

        public IExtendedCommand DeleteCommand { get; }


        public IApi Api { get; }

        public UIHost UIHost { get; }

        protected EntityViewModel(Guid id, IApi api, UIHost uiHost)
        {
            Id = id;

            Api = api;
            UIHost = uiHost;

            ShowEditorCommand = new RelayCommand(ShowEditor);
            DeleteCommand = new AsyncRelayCommand(Delete, CanDelete);
        }

        public abstract void Update(TEntity entity);


        protected abstract void ShowEditor();

        protected abstract Task Delete();

        protected abstract bool CanDelete();
    }
}
