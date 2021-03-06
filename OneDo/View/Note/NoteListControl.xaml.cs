﻿using OneDo.View.Core;
using OneDo.View.Behaviors;
using OneDo.View.Converters;
using OneDo.ViewModel;
using OneDo.ViewModel.Note;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace OneDo.View.Note
{
    public sealed partial class NoteListControl : ExtendedUserControl, IView<NoteListViewModel>
    {
        public NoteListViewModel VM => (NoteListViewModel)ViewModel;

        public NoteListControl()
        {
            InitializeComponent();
        }

        private void NoteMenuFlyout_Opening(object sender, object e)
        {
            if (VM == null)
            {
                return;
            }

            var menu = sender as MenuFlyout;
            var rootItem = menu.Items.FirstOrDefault(x => x.Name == "MoveToFolderRootMenuItem") as MenuFlyoutSubItem;
            var note = FlyoutMenuItemBehavior.GetSubItemContext(rootItem) as NoteItemViewModel;

            rootItem.Items.Clear();
            foreach (var folder in VM.FolderList.Items.Where(x => x.Id != note.FolderId))
            {
                var item = new MenuFlyoutItem
                {
                    Command = note.MoveToFolderCommand,
                    CommandParameter = folder.Id,
                };

                BindingOperations.SetBinding(item, MenuFlyoutItem.TextProperty, new Binding
                {
                    Path = new PropertyPath(nameof(folder.Name)),
                    Source = folder,
                    Mode = BindingMode.OneWay,
                });
                BindingOperations.SetBinding(item, MenuFlyoutItem.ForegroundProperty, new Binding
                {
                    Path = new PropertyPath(nameof(folder.Color)),
                    Source = folder,
                    Mode = BindingMode.OneWay,
                    Converter = Windows.UI.Xaml.Application.Current.Resources[nameof(ColorToBrushConverter)] as ColorToBrushConverter,
                });

                rootItem.Items.Add(item);
            }
        }
    }
}
