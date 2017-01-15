﻿using OneDo.Application.Core;
using OneDo.Application.Queries;
using OneDo.Application.Queries.Folders;
using OneDo.Application.Queries.Notes;
using OneDo.Data.Entities;
using OneDo.Data.Services.DataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Application
{
    public class Api
    {
        public ICommandBus CommandBus { get; }

        public IFolderQuery FolderQuery { get; }

        public INoteQuery NoteQuery { get; }

        private readonly IDataService dataService;

        public Api()
        {
            dataService = new DataService();

            CommandBus = new CommandBus(dataService);

            FolderQuery = new FolderQuery(dataService.RepositoryFactory.GetQueryRepository<FolderData>());
            NoteQuery = new NoteQuery(dataService.RepositoryFactory.GetQueryRepository<NoteData>());
        }
    }
}