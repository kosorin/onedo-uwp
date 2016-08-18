using GalaSoft.MvvmLight.Command;
using OneDo.Common.Event;
using OneDo.Common.Logging;
using OneDo.Common.Media;
using OneDo.Model.Business;
using OneDo.Model.Business.Validation;
using OneDo.Model.Data;
using OneDo.Model.Data.Entities;
using OneDo.Services.ModalService;
using OneDo.Services.ProgressService;
using OneDo.ViewModel.Commands;
using OneDo.ViewModel.Items;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI.Xaml.Navigation;

namespace OneDo.ViewModel.Modals
{
    public class FolderEditorViewModel : ModalViewModel
    {
        private bool isNew;
        public bool IsNew
        {
            get { return isNew; }
            set { Set(ref isNew, value); }
        }

        private bool isDirty;
        public bool IsDirty
        {
            get { return isDirty; }
            set { Set(ref isDirty, value); }
        }


        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (Set(ref name, value))
                {
                    IsDirty = true;
                }
            }
        }

        private List<ColorItem> colors;
        public List<ColorItem> Colors
        {
            get { return colors; }
            set { Set(ref colors, value); }
        }

        private ColorItem selectedColor;
        public ColorItem SelectedColor
        {
            get { return selectedColor; }
            set
            {
                if (Set(ref selectedColor, value))
                {
                    IsDirty = true;
                }
            }
        }


        public event TypedEventHandler<FolderEditorViewModel, FolderEventArgs> Deleted;

        private void OnDeleted()
        {
            Deleted?.Invoke(this, new FolderEventArgs(original));
        }


        public event TypedEventHandler<FolderEditorViewModel, FolderEventArgs> Saved;

        private void OnSaved()
        {
            Saved?.Invoke(this, new FolderEventArgs(original));
        }


        public ICommand DeleteCommand { get; }

        public ICommand SaveCommand { get; }

        public IProgressService ProgressService { get; }

        private readonly Folder original;

        private readonly FolderBusiness business;

        public FolderEditorViewModel(IModalService modalService, ISettingsProvider settingsProvider, IProgressService progressService, Folder folder) : base(modalService, settingsProvider)
        {
            ProgressService = progressService;

            business = new FolderBusiness(SettingsProvider);
            original = folder ?? business.Default();

            DeleteCommand = new AsyncRelayCommand(Delete);
            SaveCommand = new AsyncRelayCommand(Save);

            Colors = new List<ColorItem>
            {
                new ColorItem(ColorHelper.FromHex("#E81123"), ""),
                new ColorItem(ColorHelper.FromHex("#004E8C"), ""),
                new ColorItem(ColorHelper.FromHex("#0F893E"), ""),
                new ColorItem(ColorHelper.FromHex("#F7630D"), ""),
                new ColorItem(ColorHelper.FromHex("#C10051"), ""),
                new ColorItem(ColorHelper.FromHex("#0063AF"), ""),
                new ColorItem(ColorHelper.FromHex("#00AC56"), ""),
                new ColorItem(ColorHelper.FromHex("#FABD14"), ""),
                new ColorItem(ColorHelper.FromHex("#AC008C"), ""),
                new ColorItem(ColorHelper.FromHex("#5B2D90"), ""),
                new ColorItem(ColorHelper.FromHex("#7EC500"), ""),
                new ColorItem(ColorHelper.FromHex("#C6A477"), ""),
                new ColorItem(ColorHelper.FromHex("#D40078"), ""),
                new ColorItem(ColorHelper.FromHex("#0099BB"), ""),
                new ColorItem(ColorHelper.FromHex("#00B6C1"), ""),
                new ColorItem(ColorHelper.FromHex("#84939A"), ""),
            };

            Load();
        }


        private void Load()
        {
            IsNew = business.IsNew(original);

            Name = original.Name;
            SelectedColor = Colors.Where(x => x.Color == ColorHelper.FromHex(original.Color)).FirstOrDefault();

            IsDirty = IsNew;
        }

        private async Task Delete()
        {
            if (!IsNew)
            {
                try
                {
                    ProgressService.IsBusy = true;
                    using (var dc = new DataContext())
                    {
                        dc.Set<Folder>().Attach(original);
                        dc.Set<Folder>().Remove(original);
                        await dc.SaveChangesAsync();
                    }
                }
                finally
                {
                    ProgressService.IsBusy = false;
                }

                OnDeleted();
            }
        }

        private async Task Save()
        {
            original.Name = Name;
            original.Color = ColorHelper.ToHex(SelectedColor.Color);

            try
            {
                ProgressService.IsBusy = true;
                using (var dc = new DataContext())
                {
                    if (IsNew)
                    {
                        dc.Set<Folder>().Add(original);
                    }
                    else
                    {
                        dc.Set<Folder>().Update(original);
                    }
                    await dc.SaveChangesAsync();
                }
            }
            finally
            {
                ProgressService.IsBusy = false;
            }

            OnSaved();
        }
    }
}
