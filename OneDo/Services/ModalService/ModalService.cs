using GalaSoft.MvvmLight;
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
using OneDo.Common.UI;
using OneDo.ViewModel.Commands;
using System.ComponentModel;

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
                    RaisePropertyChanged(nameof(CanClose));
                    CurrentChanged?.Invoke(this, EventArgs.Empty);
                    CanCloseChanged?.Invoke(this, EventArgs.Empty);
                    UpdateBackButtonVisibility();
                }
            }
        }

        public event EventHandler CurrentChanged;

        public bool CanClose => Current != null;

        public event EventHandler CanCloseChanged;

        public IExtendedCommand CloseCommand { get; }

        private IModalService attached;
        public IModalService Attached
        {
            get { return attached; }
            set
            {
                if (Set(ref attached, value))
                {
                    RaisePropertyChanged(nameof(CanClose));
                    UpdateBackButtonVisibility();
                }
            }
        }


        private SystemNavigationManager navigationManager;

        [Obsolete("Do not use this constructor")]
        public ModalService()
        {
            // Dummy constructor
        }

        public ModalService(Window window, SystemNavigationManager navigationManager)
        {
            window.CoreWindow.PointerPressed += OnPointerPressed;
            window.CoreWindow.KeyDown += OnKeyDown;

            this.navigationManager = navigationManager;
            this.navigationManager.BackRequested += OnBackRequested;

            CloseCommand = new RelayCommand(Close, () => CanClose);
        }


        public bool TryClose()
        {
            if (CanClose)
            {
                if (Attached?.CanClose ?? false)
                {
                    Attached.Close();
                }
                else
                {
                    Close();
                }
                return true;
            }
            return false;
        }

        public void Close()
        {
            Detach();
            Current = null;
        }

        public void Show(ModalViewModel modal)
        {
            Detach();
            Current = modal;
            Attach(modal.SubModalService);
        }


        private void Detach()
        {
            if (Attached != null)
            {
                Attached.CanCloseChanged -= Attached_CanCloseChanged;
            }
            Attached = null;
        }

        private void Attach(IModalService modalService)
        {
            Detach();
            Attached = modalService;
            if (Attached != null)
            {
                Attached.CanCloseChanged += Attached_CanCloseChanged;
            }
        }

        private void Attached_CanCloseChanged(object sender, EventArgs e)
        {
            UpdateBackButtonVisibility();
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
            navigationManager.AppViewBackButtonVisibility = (CanClose || (Attached?.CanClose ?? false))
                ? AppViewBackButtonVisibility.Visible
                : AppViewBackButtonVisibility.Collapsed;
        }
    }
}
