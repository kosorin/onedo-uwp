using System.Threading.Tasks;
using SQLite.Net;
using OneDo.Model.Data.Entities;

namespace OneDo.Model.Data
{
    public interface IDataService
    {
        Repository<Folder> Folders { get; }

        Repository<Note> Notes { get; }

        Task InitializeAsync();


        Settings Settings { get; }

        Task LoadSettingsAsync();

        Task SaveSettingsAsync();
    }
}