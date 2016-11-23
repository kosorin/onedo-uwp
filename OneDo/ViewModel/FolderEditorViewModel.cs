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
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI.Xaml.Navigation;

namespace OneDo.ViewModel
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
                if (Set(ref name, DataService.Folders.NormalizeName(value)))
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


        public DataService DataService { get; }

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

        public FolderEditorViewModel(DataService dataService, IProgressService progressService)
            : this(dataService, progressService, null)
        {

        }

        public FolderEditorViewModel(DataService dataService, IProgressService progressService, Folder folder)
            : base(progressService)
        {
            DataService = dataService;
            original = folder ?? DataService.Folders.CreateDefault();

            Load();
        }

        private void Load()
        {
            IsNew = DataService.Folders.IsNew(original);

            Name = original.Name;
            SelectedColor = Colors
                .Where(x => x.Color.ToHex() == original.Color)
                .FirstOrDefault() ?? Colors[random.Next(Colors.Count)];
        }


        protected override async Task Save()
        {
            original.Name = DataService.Folders.NormalizeName(Name);
            original.Color = SelectedColor.Color.ToHex();

            await ProgressService.RunAsync(async () =>
            {
                await DataService.Folders.Save(original);
            });

            OnSaved();
        }

        protected override async Task Delete()
        {
            if (!IsNew)
            {
                await ProgressService.RunAsync(async () =>
                {
                    await DataService.Folders.Delete(original);
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
