using System;

namespace OneDo.Model.Data
{
    public partial class DataService : IDisposable
    {
        public void Dispose()
        {
            DisposeData();
            DisposeSettings();
        }
    }
}
