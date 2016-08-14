using OneDo.ViewModel.Flyouts;
using Windows.UI.Xaml.Controls;

namespace OneDo.View.Flyouts
{
    public sealed partial class TodoEditor : FlyoutBase, IXBind<TodoEditorViewModel>
    {
        public TodoEditorViewModel VM => ViewModel as TodoEditorViewModel;

        public TodoEditor()
        {
            InitializeComponent();
        }
    }
}
