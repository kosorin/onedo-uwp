using OneDo.Application.Queries.Notes;
using OneDo.Common.Extensions;
using OneDo.Core.Args;
using OneDo.View.Core;
using OneDo.View.Controls;
using OneDo.ViewModel;
using OneDo.ViewModel.Note;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace OneDo.View.Note
{
    public sealed partial class NoteEditorView : ModalView, IView<NoteEditorViewModel>
    {
        public NoteEditorViewModel VM => ViewModel as NoteEditorViewModel;

        public Guid? NoteId { get; }

        public NoteEditorView(Guid? noteId)
        {
            InitializeComponent();

            NoteId = noteId;
        }

        protected override async Task OnFirstLoad()
        {
            if (NoteId == null)
            {
                TitleTextBox.Focus(FocusState.Programmatic);
            }
            await VM.Load(NoteId);
        }
    }
}
