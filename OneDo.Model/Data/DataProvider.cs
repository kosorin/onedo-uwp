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
    public class DataProvider : IDataProvider
    {
        private const string FileName = "Data.json";


        public Settings Settings { get; private set; }

        public TagRepository Tags { get; private set; }

        public TodoRepository Todos { get; private set; }


        public async Task LoadAsync()
        {
            try
            {
                var data = await Serialization.DeserializeFromFileAsync<Data>(FileName, ApplicationData.Current.LocalFolder) ?? new Data();

                Settings = data.Settings;
                Tags = new TagRepository(data.Tags);
                Todos = new TodoRepository(data.Todos);

                Logger.Current.Info($"Data loaded from file '{FileName}'");
            }
            catch (Exception e)
            {
                Settings = new Settings();
                Tags = new TagRepository();
                Todos = new TodoRepository();

                Logger.Current.Error($"Loading data from file '{FileName}' failed", e);
            }
        }

        public async Task SaveAsync()
        {
            try
            {
                var data = new Data
                {
                    Settings = Settings,
                    Tags = new List<Tag>(Tags.GetAll()),
                    Todos = new List<Todo>(Todos.GetAll()),
                };

                await Serialization.SerializeToFileAsync(data, FileName, ApplicationData.Current.LocalFolder);
                Logger.Current.Info($"Data saved to file '{FileName}'");
            }
            catch (Exception e)
            {
                Logger.Current.Error($"Saving data to file '{FileName}' failed", e);
            }
        }

        private class Data
        {
            public Settings Settings { get; set; } = new Settings();

            public List<Tag> Tags { get; set; } = new List<Tag>();

            public List<Todo> Todos { get; set; } = new List<Todo>();
        }
    }
}
