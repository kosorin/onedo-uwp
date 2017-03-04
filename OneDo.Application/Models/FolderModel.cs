using OneDo.Application.Common;
using OneDo.Domain.Model.Entities;
using OneDo.Domain.Model.ValueObjects;
using OneDo.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Application.Models
{
    public class FolderModel : IModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }


        internal Folder ToEntity()
        {
            return new Folder(Id, Name, new Color(Color));
        }

        internal static FolderModel FromData(FolderData folderData)
        {
            return new FolderModel
            {
                Id = folderData.Id,
                Name = folderData.Name,
                Color = folderData.Color,
            };
        }
    }
}
