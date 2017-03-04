using OneDo.Domain.Model.Entities;
using OneDo.Domain.Model.ValueObjects;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Infrastructure.Data.Entities
{
    [DebuggerDisplay("{Id}: {Name}")]
    [Table("Folders")]
    public class FolderData : IEntityData
    {
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        public Guid Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public string Color { get; set; }


        public Folder ToEntity()
        {
            return new Folder(Id, Name, new Color(Color));
        }

        public static FolderData FromEntity(Folder folder)
        {
            return new FolderData
            {
                Id = folder.Id,
                Name = folder.Name,
                Color = folder.Color.Hex,
            };
        }
    }
}
