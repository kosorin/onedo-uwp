using OneDo.Common.Logging;
using OneDo.Model.Business.Validation;
using OneDo.Model.Data;
using OneDo.Model.Data.Objects;
using OneDo.Services.Context;
using OneDo.Services.NavigationService;
using System;
using System.Globalization;
using System.Linq;
using Windows.UI.Xaml.Navigation;

namespace OneDo.ViewModels.Editors
{
    public class TodoEditorViewModel : EditorViewModel
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


        private Todo original;

        public TodoEditorViewModel(IDataProvider dataProvider, IContext context)
            : base(dataProvider, context)
        {
            Initialize();
        }

        private void Initialize()
        {
            original = DataProvider.Todos.GetById(Context.TodoId);
            var todo = original ?? new Todo(); // TODO: vytvořit úkol s výchozími hodnotami

            IsNew = todo.Id == Guid.Empty;

            Title = todo.Title;
            Note = todo.Note;
            Date = todo.Date;
        }
    }
}