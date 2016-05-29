using OneDo.ViewModels.Editors;
using Windows.UI.Xaml.Controls;

namespace OneDo.Views.Editors
{
    public sealed partial class TodoEditor : IXBind<TodoViewModel>
    {
        public TodoViewModel VM => ViewModel as TodoViewModel;

        public TodoEditor()
        {
            InitializeComponent();
        }
    }
}
