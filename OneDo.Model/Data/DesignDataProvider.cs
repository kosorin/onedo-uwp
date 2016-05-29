using OneDo.Common.Data;
using OneDo.Common.Logging;
using OneDo.Model.Data.Objects;
using OneDo.Model.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace OneDo.Model.Data
{
    public class DesignDataProvider : IDataProvider
    {
        public Settings Settings { get; } = new Settings();

        public TagRepository Tags { get; }

        public TodoRepository Todos { get; }

        public DesignDataProvider()
        {
            Tags = new TagRepository();
            Todos = new TodoRepository();

            Todos.AddOrUpdate(new Todo
            {
                Title = "Testovací úkol",
                Flag = true,
            });
            Todos.AddOrUpdate(new Todo
            {
                Title = "Vyvenčit Bena",
                Date = DateTime.Today,
            });
            Todos.AddOrUpdate(new Todo
            {
                Title = "Koupit mléko",
                Flag = true,
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