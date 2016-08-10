using OneDo.Services.NavigationService;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using OneDo.Model.Data;
using System.Collections.Generic;
using System.Linq;
using OneDo.ViewModels.Items;
using OneDo.ViewModels.Flyouts;
using Windows.UI.Core;
using OneDo.Model.Data.Entities;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using OneDo.ViewModels.Commands;

namespace OneDo.ViewModels.Pages
{
    public class MainViewModel : PageViewModel
    {
        private ObservableCollection<TodoItemViewModel> todoItems;
        public ObservableCollection<TodoItemViewModel> TodoItems
        {
            get { return todoItems; }
            set { Set(ref todoItems, value); }
        }

        private ObservableCollection<FolderItemViewModel> folderItems;
        public ObservableCollection<FolderItemViewModel> FolderItems
        {
            get { return folderItems; }
            set { Set(ref folderItems, value); }
        }


        private TodoItemViewModel selectedTodoItem;
        public TodoItemViewModel SelectedTodoItem
        {
            get { return selectedTodoItem; }
            set { Set(ref selectedTodoItem, value); }
        }

        private FolderItemViewModel selectedFolderItem;
        public FolderItemViewModel SelectedFolderItem
        {
            get { return selectedFolderItem; }
            set { Set(ref selectedFolderItem, value); }
        }


        public ICommand TodoItemTappedCommand { get; }

        public ICommand AddTodoCommand { get; }

        public ICommand ResetDataCommand { get; }

        public ICommand ShowSettingsCommand { get; }

        public MainViewModel(INavigationService navigationService, ISettingsProvider settingsProvider)
            : base(navigationService, settingsProvider)
        {
            TodoItemTappedCommand = new RelayCommand(TodoItemTapped);
            AddTodoCommand = new RelayCommand(AddTodo);
            ResetDataCommand = new AsyncRelayCommand(ResetData);
            ShowSettingsCommand = new RelayCommand(ShowSettings);

            LoadData();
        }

        private async Task LoadData()
        {
            using (var dc = new OneDoContext())
            {
                var folders = await dc.Set<Folder>().ToListAsync();
                var folderItems = folders.Select(f => new FolderItemViewModel(f));
                FolderItems = new ObservableCollection<FolderItemViewModel>(folderItems);

                var todos = await dc.Set<Todo>().ToListAsync();
                var todoItems = todos.Select(t => new TodoItemViewModel(t));
                TodoItems = new ObservableCollection<TodoItemViewModel>(todoItems);
            }
        }

        private async Task ResetData()
        {
            using (var dc = new OneDoContext())
            {
                var todos = TodoItems.Select(x => x.Todo);
                dc.Set<Todo>().AttachRange(todos);
                dc.Set<Todo>().RemoveRange(todos);
                await dc.SaveChangesAsync();
                TodoItems.Clear();
            }
        }

        private void AddTodo()
        {
            var editor = new TodoEditorViewModel(NavigationService, SettingsProvider, null);
            editor.Saved += (s, e) => TodoItems.Add(new TodoItemViewModel(e.Todo));
            ShowTodoEditor(editor);
        }

        private void TodoItemTapped()
        {
            if (SelectedTodoItem != null)
            {
                var editor = new TodoEditorViewModel(NavigationService, SettingsProvider, SelectedTodoItem.Todo);
                editor.Deleted += (s, e) => TodoItems.Remove(SelectedTodoItem);
                editor.Saved += (s, e) => SelectedTodoItem.Refresh();
                ShowTodoEditor(editor);
            }
        }

        private void ShowTodoEditor(TodoEditorViewModel editor)
        {
            editor.Deleted += (s, e) => NavigationService.CloseFlyout();
            editor.Saved += (s, e) => NavigationService.CloseFlyout();
            NavigationService.ShowFlyout(editor);
        }

        private void ShowSettings()
        {
            NavigationService.ShowFlyout(new SettingsViewModel(NavigationService, SettingsProvider));
        }
    }
}
