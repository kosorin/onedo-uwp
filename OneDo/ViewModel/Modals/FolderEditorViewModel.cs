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
        public static List<ColorItemObject> colors;
        public List<ColorItemObject> Colors => colors;

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (Set(ref name, business.NormalizeName(value)))
                {
                    UpdateDirtyProperty(() => Name != original.Name);
                    ValidateProperty(() => !string.IsNullOrWhiteSpace(Name));
                }
            }
        }

        private ColorItemObject selectedColor;
        public ColorItemObject SelectedColor
        {
            get { return selectedColor; }
            set
            {
                if (Set(ref selectedColor, value))
                {
                    UpdateDirtyProperty(() => SelectedColor?.Color.ToHex() != original.Color);
                }
            }
        }


        public event TypedEventHandler<FolderEditorViewModel, EntityEventArgs<Folder>> Saved;

        public event TypedEventHandler<FolderEditorViewModel, EntityEventArgs<Folder>> Deleted;


        private readonly FolderBusiness business;

        private readonly Folder original;

        private readonly Random random = new Random();

        static FolderEditorViewModel()
        {
            colors = new List<ColorItemObject>
            {
                new ColorItemObject("#C10051"),
                new ColorItemObject("#E81123"),
                new ColorItemObject("#F7630D"),
                new ColorItemObject("#FABD14"),
                new ColorItemObject("#7EC500"),
                new ColorItemObject("#0F893E"),
                new ColorItemObject("#00AC56"),
                new ColorItemObject("#00B6C1"),
                new ColorItemObject("#0099BB"),
                new ColorItemObject("#0063AF"),
                new ColorItemObject("#004E8C"),
                new ColorItemObject("#5B2D90"),
                new ColorItemObject("#AC008C"),
                new ColorItemObject("#D40078"),
                new ColorItemObject("#C6A477"),
                new ColorItemObject("#84939A"),
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

            await ProgressService.RunAsync(async () =>
            {
                using (var dc = new DataContext())
                {
                    if (IsNew)
                    {
                        dc.Folders.Add(original);
                    }
                    else
                    {
                        dc.Folders.Update(original);
                    }
                    await dc.SaveChangesAsync();
                }
            });

            OnSaved();
        }

        protected override async Task Delete()
        {
            if (!IsNew)
            {
                await ProgressService.RunAsync(async () =>
                {
                    using (var dc = new DataContext())
                    {
                        dc.Folders.Attach(original);
                        dc.Folders.Remove(original);
                        await dc.SaveChangesAsync();
                    }
                });

                OnDeleted();
            }
        }

        protected override void OnSaved()
        {
            Saved?.Invoke(this, new EntityEventArgs<Folder>(original));
        }

        protected override void OnDeleted()
        {
            Deleted?.Invoke(this, new EntityEventArgs<Folder>(original));
        }
    }
}
