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
    public class ModalService : ExtendedViewModel, IModalService
    {
        public ObservableCollection<ModalViewModel> Items { get; } = new ObservableCollection<ModalViewModel>();

        private ModalViewModel current;
        public ModalViewModel Current
        {
            get { return current; }
            private set { Set(ref current, value); }
        }

        private bool canCloseCurrent;
        public bool CanCloseCurrent
        {
            get { return canCloseCurrent; }
            set
            {
                if (Set(ref canCloseCurrent, value))
                {
                    OnCanCloseCurrentChanged();
                }
            }
        }

        public event EventHandler CanCloseCurrentChanged;

        public ICommand CloseCurrentCommand { get; }


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

            CloseCurrentCommand = new RelayCommand(CloseCurrent, () => CanCloseCurrent);
        }

        public bool TryCloseCurrent()
        {
            if (CanCloseCurrent)
            {
                CloseCurrent();
                return true;
            }
            return false;
        }

        public void CloseCurrent()
        {
            Items.RemoveAt(Items.Count - 1);
        }

        public void Show(ModalViewModel modal)
        {
            Items.Add(modal);
        }


        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CanCloseCurrent = Items.Count > 0;
            Current = Items.FirstOrDefault();
        }

        private void OnCanCloseCurrentChanged()
        {
            CanCloseCurrentChanged?.Invoke(this, EventArgs.Empty);
            UpdateBackButtonVisibility();
        }

        private void OnPointerPressed(CoreWindow sender, PointerEventArgs args)
        {
            var pointer = args.CurrentPoint;
            if (pointer.Properties.IsXButton1Pressed)
            {
                if (TryCloseCurrent())
                {
                    args.Handled = true;
                }
            }
        }

        private void OnKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.Escape)
            {
                if (TryCloseCurrent())
                {
                    args.Handled = true;
                }
            }
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (TryCloseCurrent())
            {
                e.Handled = true;
            }
        }


        private void UpdateBackButtonVisibility()
        {
            navigationManager.AppViewBackButtonVisibility = CanCloseCurrent
                ? AppViewBackButtonVisibility.Visible
                : AppViewBackButtonVisibility.Collapsed;
        }
    }
}
