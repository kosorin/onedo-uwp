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

        private Data data = null;


        public Settings Settings => data.Settings;

        public TagRepository Tags { get; private set; }

        public TodoRepository Todos { get; private set; }


        public async Task LoadAsync()
        {
            try
            {
                data = await Serialization.DeserializeFromFileAsync<Data>(FileName, ApplicationData.Current.LocalFolder) ?? new Data();

                Tags = new TagRepository(data.Tags);
                Todos = new TodoRepository(data.Todos);

                Logger.Current.Info($"Data loaded from file '{FileName}'");
            }
            catch (Exception e)
            {
                data = new Data();
                Logger.Current.Error($"Loading data from file '{FileName}' failed", e);
            }
        }

        public async Task SaveAsync()
        {
            try
            {
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
