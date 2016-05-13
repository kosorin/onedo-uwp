using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OneDo.Model;
using Windows.UI;

namespace OneDo.Services.DataService
{
    public class DesignDataService : IDataService
    {
        public Task<Data> LoadAsync()
        {
            var tags = new List<Tag>
            {
                new Tag { Guid = Guid.NewGuid(), Name = "Práce", Color = Colors.Maroon },
                new Tag { Guid = Guid.NewGuid(), Name = "Škola", Color = Colors.Navy },
                new Tag { Guid = Guid.NewGuid(), Name = "Osobní", Color = Colors.DarkGreen },
            };

            var data = new Data
            {
                Tags = tags
            };

            return Task.FromResult(data);
        }

        public Task SaveAsync(Data data)
        {
            return Task.CompletedTask;
        }
    }
}
