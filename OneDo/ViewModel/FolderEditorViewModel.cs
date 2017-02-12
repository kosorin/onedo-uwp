using GalaSoft.MvvmLight.Messaging;
using OneDo.Application;
using OneDo.Application.Commands.Folders;
using OneDo.Application.Models;
using OneDo.Application.Queries.Folders;
using OneDo.Common.Extensions;
using OneDo.Core.CommandMessages;
using OneDo.Services.ProgressService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneDo.ViewModel
{
    public class FolderEditorViewModel : EditorViewModel<FolderModel>
    {
        public static List<ColorViewModel> colors;
        public List<ColorViewModel> Colors => colors;

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

        private ColorViewModel selectedColor;
        public ColorViewModel SelectedColor
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
            colors = new List<ColorViewModel>
            {
                new ColorViewModel("#C10051"),
                new ColorViewModel("#E81123"),
                new ColorViewModel("#F7630D"),
                new ColorViewModel("#FABD14"),
                new ColorViewModel("#7EC500"),
                new ColorViewModel("#0F893E"),
                new ColorViewModel("#00AC56"),
                new ColorViewModel("#00B6C1"),
                new ColorViewModel("#0099BB"),
                new ColorViewModel("#0063AF"),
                new ColorViewModel("#004E8C"),
                new ColorViewModel("#5B2D90"),
                new ColorViewModel("#AC008C"),
                new ColorViewModel("#D40078"),
                new ColorViewModel("#C6A477"),
                new ColorViewModel("#84939A"),
            };
        }

        public FolderEditorViewModel(IApi api, IProgressService progressService) : base(api, progressService)
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
                await Api.CommandBus.Execute(new SaveFolderCommand(Original));
                Messenger.Default.Send(new CloseModalMessage());
            });
        }

        protected override async Task Delete()
        {
            await ProgressService.RunAsync(async () =>
            {
                await Api.CommandBus.Execute(new DeleteFolderCommand(Original.Id));
                Messenger.Default.Send(new CloseModalMessage());
            });
        }
    }
}
