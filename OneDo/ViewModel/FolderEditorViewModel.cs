using GalaSoft.MvvmLight.Command;
using OneDo.Common.Logging;
using OneDo.Common.Media;
using OneDo.Model.Business;
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
                if (Set(ref name, Business.NormalizeName(value)))
                {
                    UpdateDirtyProperty(() => Name != Original.Name);
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
                    UpdateDirtyProperty(() => SelectedColor?.Color.ToHex() != Original.Color);
                }
            }
        }


        public DataService DataService { get; }

        public FolderBusiness Business { get; }

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
            Business = new FolderBusiness(DataService);
            Original = folder ?? Business.CreateDefault();

            Load();
        }


        private void Load()
        {
            IsNew = DataService.Folders.IsNew(Original);

            Name = Original.Name;
            SelectedColor = Colors
                .Where(x => x.Color.ToHex() == Original.Color)
                .FirstOrDefault() ?? Colors[random.Next(Colors.Count)];
        }

        protected override async Task Save()
        {
            Original.Name = Business.NormalizeName(Name);
            Original.Color = SelectedColor.Color.ToHex();

            if (IsNew)
            {
                Original.Created = DateTime.Now;
            }
            Original.Modified = DateTime.Now;

            await ProgressService.RunAsync(async () =>
            {
                await DataService.Folders.AddOrUpdate(Original);
            });
            OnSaved();
        }

        protected override async Task Delete()
        {
            await ProgressService.RunAsync(async () =>
            {
                await DataService.Folders.Delete(Original);
            });
            OnDeleted();
        }
    }
}
