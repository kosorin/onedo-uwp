using System.Threading.Tasks;

namespace OneDo.Model.Data
{
    public interface IDataProvider
    {
        Data Data { get; set; }

        Task LoadAsync();

        Task SaveAsync();
    }
}