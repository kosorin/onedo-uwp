using OneDo.Common.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace OneDo.Services.FileService
{
    public interface IFileService
    {
        Task<bool> FileExistsAsync(string key, ApplicationDataLocality locality = ApplicationDataLocality.Local);

        Task<bool> DeleteFileAsync(string key, ApplicationDataLocality locality = ApplicationDataLocality.Local);

        Task<T> ReadFileAsync<T>(string key, ApplicationDataLocality locality = ApplicationDataLocality.Local);

        Task<bool> WriteFileAsync<T>(string key, T value, ApplicationDataLocality locality = ApplicationDataLocality.Local, CreationCollisionOption option = CreationCollisionOption.ReplaceExisting);
    }
}
