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

        public bool IsLoaded { get; private set; }

        public Data Data { get; private set; }

        public async Task LoadAsync()
        {
            Data = await FileHelper.ReadFileAsync<Data>(FileName);
            await Task.Delay(2000);
            IsLoaded = true;
            Loaded?.Invoke(this, new EventArgs());
        }

        public async Task SaveAsync()
        {
            await FileHelper.WriteFileAsync(FileName, Data ?? new Data());
            Saved?.Invoke(this, new EventArgs());
        }

        public event EventHandler Loaded;

        public event EventHandler Saved;
    }
}
