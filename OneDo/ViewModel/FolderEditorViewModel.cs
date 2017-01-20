using OneDo.Application;
using OneDo.Application.Commands.Folders;
using OneDo.Application.Queries.Folders;
using OneDo.Common.Extensions;
using OneDo.Services.ProgressService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneDo.ViewModel
{
    public class FolderEditorViewModel : EditorViewModel<FolderModel>
    {
        public static List<ColorItemObject> colors;
        public List<ColorItemObject> Colors => colors;

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (Set(ref name, value))
                {
                    UpdateDirtyProperty(() => string.IsNullOrWhiteSpace(Name) != string.IsNullOrWhiteSpace(Original.Name) || Name != Original.Name);
                }
                ValidateProperty(() => !string.IsNullOrWhiteSpace(Name));
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
                ValidateProperty(() => SelectedColor != null);
            }
        }


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

        public FolderEditorViewModel(Api api, IProgressService progressService) : base(api, progressService)
        {
        }

        protected override void Load()
        {
            Name = Original.Name;
            SelectedColor = Colors
                .Where(x => x.Color.ToHex() == Original.Color)
                .FirstOrDefault() ?? Colors[random.Next(Colors.Count)];
        }


        protected override async Task Save()
        {
            Original.Name = Name;
            Original.Color = SelectedColor.Color.ToHex();

            await ProgressService.RunAsync(async () =>
            {
                await Api.CommandBus.Execute(new SaveFolderCommand(Original.Id, Original.Name, Original.Color));
            });
            OnSaved();
        }

        protected override async Task Delete()
        {
            await ProgressService.RunAsync(async () =>
            {
                await Api.CommandBus.Execute(new DeleteFolderCommand(Original.Id));
            });
            OnDeleted();
        }
    }
}
