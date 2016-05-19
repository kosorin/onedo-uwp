using OneDo.Common.Data;
using OneDo.Common.Logging;
using OneDo.Model.Data.Objects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace OneDo.Model.Data
{
    public class DesignDataProvider : IDataProvider
    {
        public Settings Settings { get; set; } = new Settings();

        public List<Tag> Tags { get; set; } = new List<Tag>();

        public List<Todo> Todos { get; set; } = new List<Todo>();

        public DesignDataProvider()
        {
            Todos.Add(new Todo
            {
                Title = "Karel",
            });
        }

        public Task LoadAsync()
        {
            return Task.CompletedTask;
        }

        public Task SaveAsync()
        {
            return Task.CompletedTask;
        }
    }
}