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
        Data Data { get; }

        Task LoadAsync();

        Task SaveAsync();
    }
}
