using OneDo.Model.Data.Objects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OneDo.Model.Data
{
    public interface IDataProvider
    {
        Settings Settings { get; }

        List<Tag> Tags { get; }

        List<Todo> Todos { get; }

        Task LoadAsync();

        Task SaveAsync();
    }
}