using OneDo.ViewModel.Modals;
using Windows.UI.Xaml.Controls;

namespace OneDo.View.Modals
{
    public sealed partial class TodoEditor : ModalBase, IXBind<TodoEditorViewModel>
    {
        public TodoEditorViewModel VM => ViewModel as TodoEditorViewModel;

        public TodoEditor()
        {
            InitializeComponent();
        }
    }
}
