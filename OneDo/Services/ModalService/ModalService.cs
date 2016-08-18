using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using OneDo.ViewModel;
using OneDo.View;
using System;
using System.Linq;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OneDo.Common.Data;
using System.Collections.Specialized;
using Windows.System;

namespace OneDo.Services.ModalService
{
    public class ModalService : ViewModelBase, IModalService
    {
        public ObservableCollection<ModalViewModel> Items { get; } = new ObservableCollection<ModalViewModel>();

        private ModalViewModel current;
        public ModalViewModel Current
        {
            get { return current; }
            private set { Set(ref current, value); }
        }

        public bool CanPop => Items.Count > 0;


        public ICommand PopCommand { get; }


        private readonly SystemNavigationManager navigationManager;

        public ModalService()
        {
            // Dummy constructor
        }

        public ModalService(Window window, SystemNavigationManager navigationManager)
        {
            this.navigationManager = navigationManager;

            Items.CollectionChanged += OnItemsCollectionChanged;
            window.CoreWindow.PointerPressed += OnPointerPressed;
            window.CoreWindow.KeyDown += OnKeyDown;
            navigationManager.BackRequested += OnBackRequested;

            PopCommand = new RelayCommand(Pop, () => CanPop);
        }

        public bool TryPop()
        {
            if (CanPop)
            {
                Pop();
                return true;
            }
            return false;
        }

        public void Pop()
        {
            Items.RemoveAt(0);
        }

        public void Push(ModalViewModel modal)
        {
            Items.Insert(0, modal);
        }


        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Current = Items.Count > 0 ? Items.First() : null;
            UpdateBackButtonVisibility();
            RaisePropertyChanged(nameof(CanPop));
        }

        private void OnPointerPressed(CoreWindow sender, PointerEventArgs args)
        {
            var pointer = args.CurrentPoint;
            if (pointer.Properties.IsXButton1Pressed)
            {
                if (TryPop())
                {
                    args.Handled = true;
                }
            }
        }

        private void OnKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.Escape)
            {
                if (TryPop())
                {
                    args.Handled = true;
                }
            }
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (TryPop())
            {
                e.Handled = true;
            }
        }


        private void UpdateBackButtonVisibility()
        {
            navigationManager.AppViewBackButtonVisibility = CanPop
                ? AppViewBackButtonVisibility.Visible
                : AppViewBackButtonVisibility.Collapsed;
        }
    }
}
