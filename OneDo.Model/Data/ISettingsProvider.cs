using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Model.Data
{
    public interface ISettingsProvider
    {
        Settings Current { get; }

        Task LoadAsync();

        Task SaveAsync();
    }
}
