using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OneDo.Common;
using OneDo.Model;
using OneDo.Common.IO;

namespace OneDo.Services.DataService
{
    public class DataService : IDataService
    {
        private const string FileName = "Data.json";

        public Data Data { get; private set; }

        public async Task LoadAsync()
        {
            Data = await FileHelper.ReadFileAsync<Data>(FileName) ?? new Data();
        }

        public async Task SaveAsync()
        {
            await FileHelper.WriteFileAsync(FileName, Data ?? new Data());
        }
    }
}
