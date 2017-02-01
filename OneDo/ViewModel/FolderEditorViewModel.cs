using OneDo.Application;
using OneDo.Application.Commands.Folders;
using OneDo.Application.Queries.Folders;
using OneDo.Common.Extensions;
using OneDo.Services.ProgressService;
using OneDo.ViewModel.Items;
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
                    ValidateProperty();
                    MarkProperty(() => Name.TrimNull() != Original.Name.TrimNull());
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
                    ValidateProperty();
                    MarkProperty(() => SelectedColor?.Color.ToHex() != Original.Color);
                }
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
            Rules = new Dictionary<string, Func<bool>>
            {
                [nameof(Name)] = () => !string.IsNullOrWhiteSpace(Name),
                [nameof(SelectedColor)] = () => SelectedColor != null,
            };
        }

        protected override async Task InitializeData()
        {
            if (Id != null)
            {
                await ProgressService.RunAsync(async () =>
                {
                    var original = await Api.FolderQuery.Get((Guid)Id);
                    if (original != null)
                    {
                        Original = original;
                    }
                });
            }
        }

        protected override void InitializeProperties()
        {
            Name = Original.Name;
            SelectedColor = Colors
                .Where(x => x.Color.ToHex() == Original.Color)
                .FirstOrDefault() ?? Colors[random.Next(Colors.Count)];
        }


        protected override async Task Save()
        {
            Original.Name = Name.TrimNull();
            Original.Color = SelectedColor.Color.ToHex();

            await ProgressService.RunAsync(async () =>
            {
                await Api.CommandBus.Execute(new SaveFolderCommand(Original.Id, Original.Name, Original.Color));
            });
        }

        protected override async Task Delete()
        {
            await ProgressService.RunAsync(async () =>
            {
                await Api.CommandBus.Execute(new DeleteFolderCommand(Original.Id));
            });
        }
    }
}
