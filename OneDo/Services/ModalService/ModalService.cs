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
        private ModalViewModel current;
        public ModalViewModel Current
        {
            get { return current; }
            private set
            {
                if (Set(ref current, value))
                {
                    CanClose = Current != null;
                    CurrentChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private bool canClose;
        public bool CanClose
        {
            get { return canClose; }
            set
            {
                if (Set(ref canClose, value))
                {
                    UpdateBackButtonVisibility();
                }
            }
        }

        public event EventHandler CurrentChanged;

        public ICommand CloseCommand { get; }


        private readonly SystemNavigationManager navigationManager;

        public ModalService()
        {
            // Dummy constructor
        }

        public ModalService(Window window, SystemNavigationManager navigationManager)
        {
            this.navigationManager = navigationManager;

            window.CoreWindow.PointerPressed += OnPointerPressed;
            window.CoreWindow.KeyDown += OnKeyDown;
            navigationManager.BackRequested += OnBackRequested;

            CloseCommand = new RelayCommand(Close, () => CanClose);
        }

        public bool TryClose()
        {
            if (CanClose)
            {
                Close();
                return true;
            }
            return false;
        }

        public void Close()
        {
            Current = null;
        }

        public void Show(ModalViewModel modal)
        {
            Current = modal;
        }


        private void OnPointerPressed(CoreWindow sender, PointerEventArgs args)
        {
            var pointer = args.CurrentPoint;
            if (pointer.Properties.IsXButton1Pressed)
            {
                if (TryClose())
                {
                    args.Handled = true;
                }
            }
        }

        private void OnKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.Escape)
            {
                if (TryClose())
                {
                    args.Handled = true;
                }
            }
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (TryClose())
            {
                e.Handled = true;
            }
        }


        private void UpdateBackButtonVisibility()
        {
            navigationManager.AppViewBackButtonVisibility = CanClose
                ? AppViewBackButtonVisibility.Visible
                : AppViewBackButtonVisibility.Collapsed;
        }
    }
}
