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

        public string Title
        {
            get { return todo.Title; }
            set
            {
                if (todo.Title != value)
                {
                    todo.Title = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Note
        {
            get { return todo.Note; }
            set
            {
                if (todo.Note != value)
                {
                    todo.Note = value;
                    RaisePropertyChanged();
                }
            }
        }

        public DateTimeOffset? Date
        {
            get { return todo.Date != null ? new DateTimeOffset(todo.Date.Value) : (DateTimeOffset?)null; }
            set
            {
                var modelValue = value?.Date;
                if (todo.Date != modelValue )
                {
                    todo.Date = modelValue;
                    RaisePropertyChanged();
                }
            }
        }


        private readonly Todo todo;
        private readonly TodoValidator validator = new TodoValidator();

        public TodoViewModel(INavigationService navigationService, IDataProvider dataProvider, IContext context)
            : base(navigationService, dataProvider)
        {
            todo = dataProvider.Todos.FirstOrDefault(t => t.Id == context.TodoId) ?? new Todo(); // TODO: vytvořit úkol s výchozími hodnotami
        }
    }
}