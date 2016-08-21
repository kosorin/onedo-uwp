using OneDo.ViewModel.Modals;
using Windows.UI.Xaml.Controls;

namespace OneDo.View.Modals
{
    public sealed partial class NoteEditor : ModalBase, IXBind<NoteEditorViewModel>
    {
        public NoteEditorViewModel VM => ViewModel as NoteEditorViewModel;

        public NoteEditor()
        {
            InitializeComponent();
        }
    }
}
