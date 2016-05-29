using OneDo.Model.Data.Objects;
using OneDo.Model.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OneDo.Model.Data
{
    public interface IDataProvider
    {
        Settings Settings { get; }

        TagRepository Tags { get; }

        TodoRepository Todos { get; }

        Task LoadAsync();

        Task SaveAsync();
    }
}