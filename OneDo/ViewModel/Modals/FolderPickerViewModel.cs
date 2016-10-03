using OneDo.Model.Data;
using OneDo.Services.ModalService;
using OneDo.Services.ProgressService;
using OneDo.ViewModel.Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace OneDo.ViewModel.Modals
{
    public class FolderPickerViewModel : ModalViewModel
    {
        public ObservableCollection<FolderItemObject> Folders { get; }

        private readonly NoteItemObject note;

        public FolderPickerViewModel(IModalService modalService, DataService dataService, IProgressService progressService, NoteItemObject note, IEnumerable<FolderItemObject> folders)
            : base(modalService, dataService)
        {
            this.note = note;
            Folders = new ObservableCollection<FolderItemObject>(folders);
        }
    }
}
