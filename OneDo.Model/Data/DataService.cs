using OneDo.Model.Data.Entities;
using SQLite.Net;
using SQLite.Net.Attributes;
using SQLite.Net.Platform.WinRT;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using SQLite.Net.Async;

namespace OneDo.Model.Data
{
    public partial class DataService : IDisposable, IDataService
    {
        public void Dispose()
        {
            DisposeData();
            DisposeSettings();
        }
    }
}
