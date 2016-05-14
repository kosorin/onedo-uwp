using System.Threading.Tasks;
using OneDo.Model;
using OneDo.Common.IO;
using System;
using OneDo.Common.Logging;

namespace OneDo.Services.DataService
{
    public class DataService : IDataService
    {
        private const string FileName = "Data.json";

        public async Task<Data> LoadAsync()
        {
            // await Task.Delay(5 * 1000);
            Data data;
            try
            {
                data = await FileHelper.ReadFileAsync<Data>(FileName);
            }
            catch (Exception e)
            {
                data = null;
                Logger.Current.Warn(e);
            }
            return data ?? new Data();
        }

        public async Task SaveAsync(Data data)
        {
            try
            {
                await FileHelper.WriteFileAsync(FileName, data ?? new Data());
            }
            catch (Exception e)
            {
                Logger.Current.Error(e);
            }
        }
    }
}
