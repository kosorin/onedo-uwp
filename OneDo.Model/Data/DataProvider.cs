using OneDo.Common.Data;
using OneDo.Common.Logging;
using OneDo.Model.Data.Objects;
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

        public List<Tag> Tags => data.Tags;

        public List<Todo> Todos => data.Todos;


        public async Task LoadAsync()
        {
            try
            {
                data = await Serialization.DeserializeFromFileAsync<Data>(FileName, ApplicationData.Current.LocalFolder) ?? new Data();
            }
            catch (Exception e)
            {
                data = new Data();
                Logger.Current.Error($"Loading '{FileName}' file failed.", e);
            }
        }

        public async Task SaveAsync()
        {
            try
            {
                await Serialization.SerializeToFileAsync(data, FileName, ApplicationData.Current.LocalFolder);
            }
            catch (Exception e)
            {
                Logger.Current.Error($"Saving '{FileName}' file failed.", e);
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
