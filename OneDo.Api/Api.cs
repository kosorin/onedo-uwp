using OneDo.Application.Core;
using OneDo.Application.Queries;
using OneDo.Application.Queries.Folders;
using OneDo.Application.Queries.Notes;
using OneDo.Infrastructure.Data;
using OneDo.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Application
{
    public class Api : IDisposable
    {
        public ICommandBus CommandBus { get; }

        public IFolderQuery FolderQuery { get; }

        public INoteQuery NoteQuery { get; }

        private readonly DataService dataService;

        public Api()
        {
            dataService = new DataService();

            CommandBus = new CommandBus(dataService);

            FolderQuery = new FolderQuery(dataService.GetQueryRepository<FolderData>());
            NoteQuery = new NoteQuery(dataService.GetQueryRepository<NoteData>());
        }


        private bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    dataService?.Dispose();
                }

                disposed = true;
            }
        }
    }
}
