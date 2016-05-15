using System.Threading.Tasks;

namespace OneDo.Model.DataAccess
{
    public interface IDataProvider
    {
        Data Data { get; set; }

        Task LoadAsync();

        Task SaveAsync();
    }
}