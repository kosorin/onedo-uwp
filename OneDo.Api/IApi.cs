using OneDo.Application.Core;
using OneDo.Application.Queries.Folders;
using OneDo.Application.Queries.Notes;

namespace OneDo.Application
{
    public interface IApi
    {
        ICommandBus CommandBus { get; }

        IFolderQuery FolderQuery { get; }

        INoteQuery NoteQuery { get; }
    }
}