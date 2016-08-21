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
    public class FolderEditorViewModel : EditorViewModel<Folder>
    {
        public static List<ColorItemViewModel> colors;
        public List<ColorItemViewModel> Colors => colors;

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (Set(ref name, business.NormalizeName(value)))
                {
                    SetDirtyProperty(() => Name != original.Name);
                }
            }
        }

        private ColorItemViewModel selectedColor;
        public ColorItemViewModel SelectedColor
        {
            get { return selectedColor; }
            set
            {
                if (Set(ref selectedColor, value))
                {
                    SetDirtyProperty(() => SelectedColor?.Color.ToHex() != original.Color);
                }
            }
        }


        public event TypedEventHandler<FolderEditorViewModel, FolderEventArgs> Saved;

        public event TypedEventHandler<FolderEditorViewModel, FolderEventArgs> Deleted;


        private readonly FolderBusiness business;

        private readonly Folder original;

        private readonly Random random = new Random();

        static FolderEditorViewModel()
        {
            colors = new List<ColorItemViewModel>
            {
                new ColorItemViewModel("#C10051"),
                new ColorItemViewModel("#E81123"),
                new ColorItemViewModel("#F7630D"),
                new ColorItemViewModel("#FABD14"),
                new ColorItemViewModel("#7EC500"),
                new ColorItemViewModel("#0F893E"),
                new ColorItemViewModel("#00AC56"),
                new ColorItemViewModel("#00B6C1"),
                new ColorItemViewModel("#0099BB"),
                new ColorItemViewModel("#0063AF"),
                new ColorItemViewModel("#004E8C"),
                new ColorItemViewModel("#5B2D90"),
                new ColorItemViewModel("#AC008C"),
                new ColorItemViewModel("#D40078"),
                new ColorItemViewModel("#C6A477"),
                new ColorItemViewModel("#84939A"),
            };
        }

        public FolderEditorViewModel(IModalService modalService, ISettingsProvider settingsProvider, IProgressService progressService)
            : this(modalService, settingsProvider, progressService, null)
        {

        }

        public FolderEditorViewModel(IModalService modalService, ISettingsProvider settingsProvider, IProgressService progressService, Folder folder)
            : base(modalService, settingsProvider, progressService)
        {
            business = new FolderBusiness(SettingsProvider);
            original = folder ?? business.Default();

            Load();
        }

        private void Load()
        {
            IsNew = business.IsNew(original);

            Name = original.Name;
            SelectedColor = Colors
                .Where(x => x.Color.ToHex() == original.Color)
                .FirstOrDefault() ?? Colors[random.Next(Colors.Count)];
        }


        protected override async Task Save()
        {
            original.Name = business.NormalizeName(Name);
            original.Color = SelectedColor.Color.ToHex();

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

        protected override async Task Delete()
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

        protected override void OnSaved()
        {
            Saved?.Invoke(this, new FolderEventArgs(original));
        }

        protected override void OnDeleted()
        {
            Deleted?.Invoke(this, new FolderEventArgs(original));
        }
    }
}
