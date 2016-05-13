
using OneDo.ViewModel;

namespace OneDo.View
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
