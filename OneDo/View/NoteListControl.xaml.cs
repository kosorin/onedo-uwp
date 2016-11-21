using OneDo.Common.UI;
using OneDo.View.Behaviors;
using OneDo.View.Converters;
using OneDo.ViewModel;
using OneDo.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace OneDo.View
{
    public sealed partial class NoteListControl : ExtendedUserControl, IXBind<NoteListViewModel>
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
            var note = FlyoutMenuItemBehavior.GetSubItemContext(rootItem) as NoteItemObject;

            rootItem.Items.Clear();
            foreach (var folder in VM.FolderList.Items)
            {
                var item = new MenuFlyoutItem
                {
                    Command = new AsyncRelayCommand(() => VM.FolderList.MoveItem(folder, note)),
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
                    Converter = Application.Current.Resources[nameof(ColorToBrushConverter)] as ColorToBrushConverter,
                });

                rootItem.Items.Add(item);
            }
        }
    }
}
