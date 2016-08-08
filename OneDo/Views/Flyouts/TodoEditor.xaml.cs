using OneDo.ViewModels.Flyouts;
using Windows.UI.Xaml.Controls;

namespace OneDo.Views.Flyouts
{
    public sealed partial class TodoEditor : IXBind<TodoEditorViewModel>
    {
        public TodoEditorViewModel VM => ViewModel as TodoEditorViewModel;

        public TodoEditor()
        {
            InitializeComponent();
        }
    }
}
