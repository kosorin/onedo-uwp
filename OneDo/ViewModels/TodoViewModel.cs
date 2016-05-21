using OneDo.Common.Logging;
using OneDo.Model.Business.Validation;
using OneDo.Model.Data;
using OneDo.Model.Data.Objects;
using OneDo.Services.Context;
using OneDo.Services.NavigationService;
using System;
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
                if (todo.Title != value && validator.IsValidProperty(todo, nameof(todo.Title)))
                {
                    todo.Title = value;
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