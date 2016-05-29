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

namespace OneDo.ViewModels
{
    public class TodoViewModel : PageViewModel
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


        private readonly Todo todo;
        private readonly TodoValidator validator = new TodoValidator();

        public TodoViewModel(INavigationService navigationService, IDataProvider dataProvider, IContext context)
            : base(navigationService, dataProvider)
        {
            todo = dataProvider.Todos.GetById(context.TodoId) ?? new Todo(); // TODO: vytvořit úkol s výchozími hodnotami
            IsNew = todo.Id == Guid.Empty;
        }
    }
}