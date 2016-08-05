using OneDo.ViewModels.Editors;
using Windows.UI.Xaml.Controls;

namespace OneDo.Views.Editors
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
