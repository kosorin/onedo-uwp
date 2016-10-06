using OneDo.Common.UI;
using OneDo.View.Behaviors;
using OneDo.ViewModel;
using OneDo.ViewModel.Commands;
using OneDo.ViewModel.Items;
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

namespace OneDo.View.Controls
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
            var menu = sender as MenuFlyout;
            var rootItem = menu.Items.FirstOrDefault(x => x.Name == "FoldersRootMenuItem") as MenuFlyoutSubItem;
            var note = FlyoutMenuItemBehavior.GetSubItemContext(rootItem) as NoteItemObject;

            rootItem.Items.Clear();
            foreach (var folder in VM.FolderList.Items)
            {
                rootItem.Items.Add(new MenuFlyoutItem
                {
                    Text = folder.Name,
                    Command = new AsyncRelayCommand(() => VM.FolderList.MoveItem(folder, note)),
                    Foreground = new SolidColorBrush(folder.Color),
                });
            }
        }
    }
}
