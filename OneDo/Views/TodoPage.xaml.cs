
using OneDo.ViewModels;

namespace OneDo.Views
{
    public sealed partial class TodoPage : IXBindPage<TodoViewModel>
    {
        public TodoViewModel VM => ViewModel as TodoViewModel;

        public TodoPage()
        {
            InitializeComponent();
        }
    }
}
