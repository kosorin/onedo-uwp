﻿using GalaSoft.MvvmLight.Command;
using OneDo.Common.Event;
using OneDo.Common.Logging;
using OneDo.Model.Business;
using OneDo.Model.Business.Validation;
using OneDo.Model.Data;
using OneDo.Model.Data.Objects;
using OneDo.Services.NavigationService;
using OneDo.ViewModels.Commands;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Navigation;

namespace OneDo.ViewModels.Flyouts
{
    public class TodoEditorViewModel : FlyoutViewModel
    {
        private bool isNew;
        public bool IsNew
        {
            get { return isNew; }
            set { Set(ref isNew, value); }
        }


        private string title;
        public string Title
        {
            get { return title; }
            set { Set(ref title, value); }
        }

        private string note;
        public string Note
        {
            get { return note; }
            set { Set(ref note, value); }
        }

        private DateTimeOffset? date;
        public DateTimeOffset? Date
        {
            get { return date; }
            set { Set(ref date, value); }
        }


        public event EventHandler<TodoEditorViewModel, TodoEventArgs> Saved;

        private void OnSaved()
        {
            Saved?.Invoke(this, new TodoEventArgs(Todo));
        }


        public ICommand SaveCommand { get; }


        public Todo Todo { get; }

        private readonly TodoBusiness business;

        public TodoEditorViewModel(INavigationService navigationService, IDataProvider dataProvider, Todo todo) : base(navigationService, dataProvider)
        {
            business = new TodoBusiness(DataProvider);

            Todo = todo ?? new Todo(); // TODO: v business vytvořit prázdný pkol

            SaveCommand = new AsyncRelayCommand(Save);

            Load();
        }


        private void Load()
        {
            IsNew = business.IsNew(Todo);

            Title = Todo.Title;
            Note = Todo.Note;
            Date = Todo.Date;
        }

        private async Task Save()
        {
            Todo.Title = Title;
            Todo.Note = Note;
            Todo.Date = Date?.DateTime;

            if (IsNew)
            {
                Todo.Id = Guid.NewGuid();
                DataProvider.Context.Todos.Add(Todo);
            }
            await DataProvider.Context.SaveChangesAsync();

            OnSaved();
        }
    }
}