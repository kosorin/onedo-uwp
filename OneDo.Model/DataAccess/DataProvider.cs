using OneDo.Common.IO;
using OneDo.Common.Logging;
using System;
using System.Threading.Tasks;

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
                Data = await FileHelper.ReadFileAsync<Data>(FileName);
            }
            catch (Exception e)
            {
                Data = null;
                Logger.Current.Error(e);
            }
        }

        public async Task SaveAsync()
        {
            try
            {
                await FileHelper.WriteFileAsync(FileName, Data);
            }
            catch (Exception e)
            {
                Logger.Current.Error(e);
            }
        }
    }
}
