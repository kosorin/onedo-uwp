using OneDo.Application.Core;
using OneDo.Application.Queries.Folders;
using OneDo.Application.Queries.Notes;
using TinyMessenger;

namespace OneDo.Application
{
    public interface IApi
    {
        EventBus EventBus { get; }

        CommandBus CommandBus { get; }

        IFolderQuery FolderQuery { get; }

        INoteQuery NoteQuery { get; }
    }
}