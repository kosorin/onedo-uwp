using GalaSoft.MvvmLight.Command;
using OneDo.Common.Event;
using OneDo.Common.Logging;
using OneDo.Model.Business;
using OneDo.Model.Business.Validation;
using OneDo.Model.Data;
using OneDo.Model.Data.Objects;
using OneDo.Services.Context;
using OneDo.Services.NavigationService;
using System;
using System.Globalization;
using System.Linq;
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


        public event EventHandler<TodoEditorViewModel, EventArgs> Saved;

        private void OnSaved()
        {
            Saved?.Invoke(this, new EventArgs());
        }


        public ICommand SaveCommand { get; }


        private readonly Todo original;

        private readonly TodoBusiness business;

        public TodoEditorViewModel(INavigationService navigationService, IDataProvider dataProvider, IContext context) : base(navigationService, dataProvider, context)
        {
            original = DataProvider.Todos.Get(Context.TodoId);
            business = new TodoBusiness(DataProvider);

            SaveCommand = new RelayCommand(Save);

            Load(original ?? new Todo());
        }


        private void Load(Todo todo)
        {
            IsNew = business.IsNew(todo);

            Title = todo.Title;
            Note = todo.Note;
            Date = todo.Date;
        }

        private void Save()
        {
            original.Title = Title;
            original.Note = Note;
            original.Date = Date?.DateTime;

            OnSaved();
            NavigationService.GoBack();
        }
    }
}