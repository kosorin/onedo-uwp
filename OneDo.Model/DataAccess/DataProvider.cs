using OneDo.Common.Data;
using OneDo.Common.Logging;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace OneDo.Model.DataAccess
{
    public class DataProvider : IDataProvider
    {
        private const string FileName = "Data.json";

        public Data Data { get; set; }

        public async Task LoadAsync()
        {
            try
            {
                Data = await Serialization.DeserializeFromFileAsync<Data>(FileName, ApplicationData.Current.LocalFolder);
            }
            catch (Exception e)
            {
                Data = null;
                Logger.Current.Error($"Loading '{FileName}' file failed.", e);
            }
        }

        public async Task SaveAsync()
        {
            try
            {
                await Serialization.SerializeToFileAsync(Data, FileName, ApplicationData.Current.LocalFolder);
            }
            catch (Exception e)
            {
                Logger.Current.Error($"Saving '{FileName}' file failed.", e);
            }
        }
    }
}
