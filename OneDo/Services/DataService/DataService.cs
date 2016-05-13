using System.Threading.Tasks;
using OneDo.Model;
using OneDo.Common.IO;

namespace OneDo.Services.DataService
{
    public class DataService : IDataService
    {
        private const string FileName = "Data.json";

        public async Task<Data> LoadAsync()
        {
            var data = await FileHelper.ReadFileAsync<Data>(FileName);
            // await Task.Delay(5 * 1000);
            return data;
        }

        public async Task SaveAsync(Data data)
        {
            await FileHelper.WriteFileAsync(FileName, data ?? new Data());
        }
    }
}
