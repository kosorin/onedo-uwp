using GalaSoft.MvvmLight.Messaging;
using OneDo.Application;
using OneDo.Application.Common;
using OneDo.Common.Mvvm;
using OneDo.Core;
using OneDo.Core.Args;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;

namespace OneDo.ViewModel
{
    public abstract class ListViewModel<TItem, TEntity> : ModalViewModel
        where TItem : EntityViewModel<TEntity>
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


        public IApi Api { get; }

        public UIHost UIHost { get; }

        protected ListViewModel(IApi api, UIHost uiHost)
        {
            Api = api;
            UIHost = uiHost;

            ShowEditorCommand = new RelayCommand(ShowEditor);
        }


        protected abstract void ShowEditor();
    }
}
