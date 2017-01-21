using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.System;
using OneDo.Common.Mvvm;

namespace OneDo.Services.ModalService
{
    [Obsolete("Otevírání Modalů se řeší přes Messanger")]
    public class ModalService : ExtendedViewModel, IModalService
    {
        private IModal current;
        public IModal Current
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

            CloseCommand = new RelayCommand(() => TryClose());
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

        public void Show(IModal modal)
        {
            Detach();
            Current = modal;
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
