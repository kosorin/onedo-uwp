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
        public FolderListViewModel FolderList { get; }

        public IProgressService ProgressService { get; }

        public NoteItemObject Note { get; }

        public FolderPickerViewModel(IModalService modalService, DataService dataService, IProgressService progressService, NoteItemObject note, FolderListViewModel folderList)
            : base(modalService, dataService)
        {
            ProgressService = progressService;

            Note = note;
            FolderList = folderList;
        }

        public async Task Pick(FolderItemObject folder)
        {
            Note.Entity.FolderId = folder.Entity.Id;
            await ProgressService.RunAsync(async () =>
            {
                await DataService.Notes.Update(Note.Entity);
            });
            Close();
        }
    }
}
