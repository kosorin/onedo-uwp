﻿using OneDo.Application.Common;
using OneDo.Application.Models;
using OneDo.Common.Extensions;
using OneDo.Infrastructure.Data.Entities;
using OneDo.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Application.Queries.Folders
{
    public class FolderQuery : IFolderQuery
    {
        private readonly IQueryRepository<FolderData> folderRepository;

        public FolderQuery(IQueryRepository<FolderData> folderRepository)
        {
            this.folderRepository = folderRepository;
        }

        public async Task<FolderModel> Get(Guid id)
        {
            var folderData = await folderRepository.Get(id);
            if (folderData != null)
            {
                return FolderModel.FromData(folderData);
            }
            else
            {
                return null;
            }
        }

        public async Task<IList<FolderModel>> GetAll()
        {
            var folderDatas = await folderRepository.GetAll();
            return folderDatas.Select(FolderModel.FromData).ToList();
        }
    }
}
