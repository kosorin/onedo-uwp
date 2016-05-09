using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneDo.Model;

namespace OneDo.Services.DataService
{
    public interface IDataService
    {
        Task<Data> LoadAsync();

        Task SaveAsync(Data data);
    }
}
