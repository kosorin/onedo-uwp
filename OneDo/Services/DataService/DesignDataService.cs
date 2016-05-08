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
        public bool IsLoaded { get; private set; }

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

            IsLoaded = true;
            Loaded?.Invoke(this, new EventArgs());
            return Task.CompletedTask;
        }

        public Task SaveAsync()
        {
            Saved?.Invoke(this, new EventArgs());
            return Task.CompletedTask;
        }

        public event EventHandler Loaded;

        public event EventHandler Saved;
    }
}
