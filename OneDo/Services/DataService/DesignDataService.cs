using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneDo.Model;
using Windows.UI;

namespace OneDo.Services.DataService
{
    public class DesignDataService : IDataService
    {
        public Data Data { get; private set; }

        public Task LoadAsync()
        {
            var tags = new List<Tag>
            {
                new Tag { Guid = Guid.NewGuid(), Name = "Práce", Color = Colors.Maroon },
                new Tag { Guid = Guid.NewGuid(), Name = "Škola", Color = Colors.Navy },
                new Tag { Guid = Guid.NewGuid(), Name = "Osobní", Color = Colors.DarkGreen },
            };

            Data = new Data
            {
                Tags = tags
            };
            return Task.CompletedTask;
        }

        public Task SaveAsync()
        {
            return Task.CompletedTask;
        }
    }
}
