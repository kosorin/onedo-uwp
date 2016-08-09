using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Model.Data
{
    public interface IDataProvider : IDisposable
    {
        OneDoContext Context { get; }

        void Initialize();

        Task SaveAsync();
    }
}
