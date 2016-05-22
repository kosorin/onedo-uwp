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
        public string Title
        {
            get { return todo.Title; }
            set
            {
                if (todo.Title != value && validator.IsPropertyValid(value, nameof(todo.Title)))
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
                if (todo.Note != value && validator.IsPropertyValid(value, nameof(todo.Note)))
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
                if (todo.Date != modelValue && validator.IsPropertyValid(modelValue, nameof(todo.Date)))
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
            todo = context.Todo ?? new Todo();  //TODO: vytvořit úkol s výchozími hodnotami
        }
    }
}